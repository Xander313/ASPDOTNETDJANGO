from django.db import models

# Create your models here.
class Impresora(models.Model):
    id=models.AutoField(primary_key=True)
    marca = models.CharField(max_length=100)
    modelo = models.CharField(max_length=100)
    ubicacion = models.CharField(max_length=150)
    fecha_adquisicion = models.CharField(max_length=100)
    imagen = models.FileField(upload_to='impresoras',null=True,blank=True)

    def __str__(self):
        return f"{self.marca} {self.modelo}"