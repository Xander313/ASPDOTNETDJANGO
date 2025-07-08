from django.core.management.base import BaseCommand
from django.conf import settings
from telegram.ext import ApplicationBuilder, CommandHandler
import nest_asyncio

# Soluciona problemas de nested event loops
nest_asyncio.apply()

class Command(BaseCommand):
    help = 'Inicia el bot de Telegram'

    def handle(self, *args, **options):
        application = ApplicationBuilder().token(settings.TELEGRAM_BOT_TOKEN).build()
        application.add_handler(CommandHandler("start", self.start))
        application.run_polling()

    async def start(self, update, context):
        await update.message.reply_text("✅ Bot funcionando con Django!")