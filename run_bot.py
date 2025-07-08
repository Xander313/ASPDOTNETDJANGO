import os
import django
import asyncio
from telegram.ext import ApplicationBuilder, CommandHandler

# Configura Django
os.environ.setdefault('DJANGO_SETTINGS_MODULE', 'Mantenimineto.settings')
django.setup()

from django.conf import settings

async def start(update, context):
    await update.message.reply_text(f"✅ Bot activo! Chat ID: {update.message.chat.id}")

async def main():
    application = ApplicationBuilder().token(settings.TELEGRAM_BOT_TOKEN).build()
    application.add_handler(CommandHandler("start", start))
    await application.run_polling()

if __name__ == "__main__":
    asyncio.run(main())