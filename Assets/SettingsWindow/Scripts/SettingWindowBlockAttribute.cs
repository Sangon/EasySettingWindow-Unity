using System;

namespace EasySettingWindow {

    [AttributeUsage(AttributeTargets.Class)]
    public class SettingWindowBlockAttribute : Attribute {

        public string Title;

        public SettingWindowBlockAttribute(string Title) {
            this.Title = Title;
        }

    }

}