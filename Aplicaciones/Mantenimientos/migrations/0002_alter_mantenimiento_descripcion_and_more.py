# Generated by Django 5.2 on 2025-06-09 14:33

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('Mantenimientos', '0001_initial'),
    ]

    operations = [
        migrations.AlterField(
            model_name='mantenimiento',
            name='descripcion',
            field=models.TextField(),
        ),
        migrations.AlterField(
            model_name='mantenimiento',
            name='id',
            field=models.AutoField(primary_key=True, serialize=False),
        ),
        migrations.AlterField(
            model_name='mantenimiento',
            name='informe_pdf',
            field=models.FileField(blank=True, null=True, upload_to='mantenimiento'),
        ),
    ]
