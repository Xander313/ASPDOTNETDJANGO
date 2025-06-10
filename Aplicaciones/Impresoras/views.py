from django.shortcuts import render, redirect
from .models import Impresora
from django.contrib import messages


# ---------------------------
def inicioI(request):
    listadoImpresoras = Impresora.objects.all()
    return render(request, "inicioI.html", {'impresoras': listadoImpresoras})

# -------------------------------
def nuevaImpresora(request):
    return render(request, "nuevaImpresora.html")

# ---------------------------------------
def guardarImpresora(request):
    marca = request.POST["marca"]
    modelo = request.POST["modelo"]
    ubicacion = request.POST["ubicacion"]
    fecha_adquisicion = request.POST["fecha_adquisicion"]
    imagen = request.FILES.get("imagen") 
     
    nuevaImpresora=Impresora.objects.create(
        marca=marca,
        modelo=modelo,
        ubicacion=ubicacion,
        fecha_adquisicion=fecha_adquisicion,
        imagen=imagen
    )
    messages.success(request, "Impresora guardada exitosamente")
    return redirect('/')

# Eliminar una impresora
def eliminarImpresora(request, id):
    impresoraEliminar = Impresora.objects.get(id=id)
    impresoraEliminar.delete()
    messages.success(request, "Impresora eliminada exitosamente")
    return redirect('/')

# Mostrar formulario para editar impresora
def editarImpresora(request, id):
    impresoraEditar = Impresora.objects.get(id=id)
    return render(request, "editarImpresora.html", {'impresoraEditar': impresoraEditar})

# Procesar edición
def procesarEdicionImpresora(request, id):
    marca = request.POST["marca"]
    modelo = request.POST["modelo"]
    ubicacion = request.POST["ubicacion"]
    fecha_adquisicion = request.POST["fecha_adquisicion"]
    imagen = request.FILES.get("imagen")

    impresora = Impresora.objects.get(id=id)
    impresora.marca = marca
    impresora.modelo = modelo
    impresora.ubicacion = ubicacion
    impresora.fecha_adquisicion = fecha_adquisicion
    if imagen:
        impresora.imagen = imagen
    impresora.save()
    messages.success(request, "Impresora actualizada exitosamente")
    return redirect('/')
