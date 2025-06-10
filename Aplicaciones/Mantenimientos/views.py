from django.shortcuts import render, redirect, get_object_or_404
from django.contrib import messages
from .models import Mantenimiento
from Aplicaciones.Impresoras.models import Impresora 
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
