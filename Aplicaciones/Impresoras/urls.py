from django.urls import path
from . import views
urlpatterns=[
    path('', views.inicioI),
    path('nuevaImpresora', views.nuevaImpresora),
    path('guardarImpresora/', views.guardarImpresora),
    path('eliminarImpresora/<id>', views.eliminarImpresora),
    path('editarImpresora/<id>', views.editarImpresora),
    path('procesarEdicionImpresora/<id>', views.procesarEdicionImpresora),
    
]