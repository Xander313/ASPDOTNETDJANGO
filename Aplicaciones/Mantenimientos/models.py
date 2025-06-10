from django.db import models
from Aplicaciones.Impresoras.models import Impresora
# Create your models here.

class Mantenimiento(models.Model):
    id=models.AutoField(primary_key=True)
    impresora = models.ForeignKey(Impresora, on_delete=models.CASCADE)
    fecha_mantenimiento = models.DateField()
    tecnico = models.CharField(max_length=100)
    descripcion = models.TextField()
    informe_pdf = models.FileField(upload_to='mantenimiento',null=True,blank=True)

    def __str__(self):
        return f"Mantenimiento {self.id} - {self.impresora}"