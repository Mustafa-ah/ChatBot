using System;
using STC.Cells;
using STC.Enums;
using STC.Models;
using Syncfusion.XForms.Chat;
using Xamarin.Forms;

namespace STC.TemplateSelector
{
    public class InquiriesTemplateSelector :ChatMessageTemplateSelector
    {
        internal SfChat Chat
        {
            get;
            set;
        }
        public InquiriesTemplateSelector(SfChat chat) : base(chat)
        {
            this.Chat = chat;
            SenderTemplate = new DataTemplate(typeof(SenderCell));
            ReceiverTemplate = new DataTemplate(typeof(ReceiverCell));
            DateTimeTemplate = new DataTemplate(typeof(DateTimeCell));
        }
        private readonly DataTemplate SenderTemplate ;

    private readonly DataTemplate ReceiverTemplate;

private readonly DataTemplate DateTimeTemplate ;

		protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
		{
            if (item is ChattingModel model)
            {
                return model.ChatType switch
                {
                    ChatListItemType.Sender => SenderTemplate,
                    ChatListItemType.Receiver => ReceiverTemplate,
                    ChatListItemType.Date => DateTimeTemplate,
                    _ => DateTimeTemplate
                };
            }
            else
            {
                return SenderTemplate;
            }
        }
	}
}
