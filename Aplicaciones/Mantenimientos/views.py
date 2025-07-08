from django.shortcuts import render, redirect, get_object_or_404
from django.contrib import messages
from .models import Mantenimiento
from Aplicaciones.Impresoras.models import Impresora 
import requests
from django.http import JsonResponse
from django.views.decorators.csrf import csrf_exempt
import json



# Listado de mantenimientos
def inicioMantenimientos(request):
    mantenimientos = Mantenimiento.objects.select_related('impresora').all()
    return render(request, 'inicioM.html', {'mantenimientos': mantenimientos})

# Mostrar formulario nuevo mantenimiento
def nuevoMantenimiento(request):
    impresoras = Impresora.objects.all()
    return render(request, 'nuevoMantenimiento.html', {'impresoras': impresoras})

# Guardar mantenimiento
def guardarMantenimiento(request):
    if request.method == 'POST':
        impresora_id = request.POST['impresora']
        fecha_mantenimiento = request.POST['fecha_mantenimiento']
        tecnico = request.POST['tecnico']
        descripcion = request.POST['descripcion']
        informe_pdf = request.FILES.get('informe_pdf')

        impresora = get_object_or_404(Impresora, id=impresora_id)

        nuevo = Mantenimiento(
            impresora=impresora,
            fecha_mantenimiento=fecha_mantenimiento,
            tecnico=tecnico,
            descripcion=descripcion,
            informe_pdf=informe_pdf
        )
        nuevo.save()
        messages.success(request, "Mantenimiento registrado exitosamente")
        return redirect('/mantenimientos/')
    else:
        return redirect('guardarMantenimiento')

# Eliminar mantenimiento
def eliminarMantenimiento(request, id):
    mantenimiento = get_object_or_404(Mantenimiento, id=id)
    mantenimiento.delete()
    messages.success(request, "Mantenimiento eliminado correctamente")
    return redirect('/mantenimientos/')

# Editar mantenimiento
def editarMantenimiento(request, id):
    mantenimiento = get_object_or_404(Mantenimiento, id=id)
    impresoras = Impresora.objects.all()
    return render(request, 'editarMantenimiento.html', {
        'mantenimiento': mantenimiento,
        'impresoras': impresoras
    })

# Procesar edición de mantenimiento
def procesarEdicionMantenimiento(request, id):
    if request.method == 'POST':
        mantenimiento = get_object_or_404(Mantenimiento, id=id)

        impresora_id = request.POST['impresora']
        fecha_mantenimiento = request.POST['fecha_mantenimiento']
        tecnico = request.POST['tecnico']
        descripcion = request.POST['descripcion']
        informe_pdf = request.FILES.get('informe_pdf')

        mantenimiento.impresora = get_object_or_404(Impresora, id=impresora_id)
        mantenimiento.fecha_mantenimiento = fecha_mantenimiento
        mantenimiento.tecnico = tecnico
        mantenimiento.descripcion = descripcion

        if informe_pdf:
            mantenimiento.informe_pdf = informe_pdf

        mantenimiento.save()
        messages.success(request, "Mantenimiento actualizado correctamente")
        return redirect('/mantenimientos/')
    else:
        return redirect('/mantenimientos/')



@csrf_exempt
def enviar_pdf_telegram(request):
    if request.method == 'POST':
        try:
            data = json.loads(request.body)
            
            if not all(key in data for key in ['pdf_url', 'chat_id', 'mensaje']):
                return JsonResponse({'status': 'error', 'message': 'Datos incompletos'}, status=400)
            
            token = "7992982183:AAH2kYLicJ5zM6NrAYExc_IowviLRJ723zo"
            
            requests.post(
                f"https://api.telegram.org/bot{token}/sendMessage",
                data={
                    'chat_id': data['chat_id'],
                    'text': data['mensaje'],
                    'parse_mode': 'Markdown'
                }
            )
            
            if data['pdf_url']:
                from django.conf import settings
                pdf_full_url = request.build_absolute_uri(data['pdf_url'])
                print("URL completa del PDF:", pdf_full_url)
                
                pdf_response = requests.get(pdf_full_url, stream=True)
                pdf_response.raise_for_status()
                
                files = {'document': ('reporte.pdf', pdf_response.content)}
                requests.post(
                    f"https://api.telegram.org/bot{token}/sendDocument",
                    data={'chat_id': data['chat_id']},
                    files=files
                )
            
            return JsonResponse({'status': 'success'})
            
        except Exception as e:
            return JsonResponse({'status': 'error', 'message': str(e)}, status=500)
    
    return JsonResponse({'status': 'error', 'message': 'Método no permitido'}, status=405)