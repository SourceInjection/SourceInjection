using System.Text;

namespace Aspects.Util
{
    internal static class StringBuilderExtensions
    {
        public static StringBuilder AppendLines(this StringBuilder builder, int count)
        {
            while(count > 0)
            {
                builder = builder.AppendLine();
                count--;
            }
            return builder;
        }
    }
}
