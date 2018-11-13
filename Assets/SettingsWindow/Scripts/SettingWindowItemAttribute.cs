using System;

namespace EasySettingWindow {
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class SettingWindowItemAttribute : Attribute {

        public string Description;

        public SettingWindowItemAttribute(string Description) {
            this.Description = Description;
        }

    }

}