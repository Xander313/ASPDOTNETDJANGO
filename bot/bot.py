from django.conf import settings
from telegram.ext import ApplicationBuilder, CommandHandler

async def start(update, context):
    await update.message.reply_text("✅ Bot conectado a Django!")

application = ApplicationBuilder().token(settings.TELEGRAM_BOT_TOKEN).build()
application.add_handler(CommandHandler("start", start)) 