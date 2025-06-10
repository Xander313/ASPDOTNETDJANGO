#configuracion de rutas
from django.urls import path
from . import views
urlpatterns=[
    path('', views.inicioMantenimientos),
    path('nuevoMantenimiento/', views.nuevoMantenimiento),
    path('guardarMantenimiento/', views.guardarMantenimiento),
    path('editarMantenimiento/<id>/', views.editarMantenimiento),
    path('procesarEdicionMantenimiento/<id>/', views.procesarEdicionMantenimiento),
    path('eliminarMantenimiento/<id>/', views.eliminarMantenimiento),
]