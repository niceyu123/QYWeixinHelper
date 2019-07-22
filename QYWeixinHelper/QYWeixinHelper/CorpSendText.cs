using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QYWeixinHelper
{
    class Text
    {
        private string _content;
        /// <summary>
        /// 要发送的文本内容字段，必须小写，企业微信API不识别大写。
        /// </summary>
        public string content
        {
            get { return _content; }
            set { _content = value; }
        }

    }
    class CorpSendText : CorpSendBase
    {
        private Text _text;
        /// <summary>
        /// 要发送的文本，必须小写，企业微信API不识别大写。
        /// </summary>
        public Text text
        {
            get { return _text; }
            set { this._text = value; }
        }


        public CorpSendText(string content)
        {
            base.msgtype = "text";
            this.text = new Text
            {
                content = content
            };
        }
    }
}
