using System;
using System.Text;

namespace Infrastructure.Common
{
    public sealed class SlugSetter
    {
        private static string[] InvalidChars = new string[] { ".", "?", "&", "/", "\\", "!" };

        public static void SetSlug(object target, string refrenceProp, string slugProp)
        {
            if (IsValidSlug(slugProp))
                return;

            if (IsValidSlug(refrenceProp))
                SetValue(target, refrenceProp);
            else
                SetValue(target, CreateValidSlug(refrenceProp));
        }


        #region private methods

        private static void SetValue(object target, string value)
        {
            const string SlugPropertyName = "Slug";

            var prop = target.GetType().GetProperty(SlugPropertyName);
            prop.SetValue(target, value.ToLowerInvariant());
        }

        private static bool IsValidSlug(string value)
        {
            if (string.IsNullOrEmpty(value)) return false;
            if (string.IsNullOrWhiteSpace(value)) return false;
            if(value.Contains(" ")) return false;

            foreach (string invalidChar in InvalidChars)
                if (value.Contains(invalidChar))
                    return false;

            return true;
        }

        private static string CreateValidSlug(string value)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                return Guid.NewGuid().ToString();

            var builder = new StringBuilder(value);
            foreach (string invalidChar in InvalidChars)
                builder.Replace(invalidChar, string.Empty);

            builder.Trim().Replace("  ", " ").Replace(" ", "-");

            return builder.ToString().ToLowerInvariant();
        }
        
        #endregion

    }
}
