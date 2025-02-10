using System.Collections.Generic;

namespace WorldMapStrategyKit {

    public struct StringSegment {
        public int startIndex;
        public int length;

        public StringSegment(int startIndex, int length) {
            this.startIndex = startIndex;
            this.length = length;
        }
    }

    public static class FastSplitString {

        public static IEnumerable<StringSegment> GetSplit(this string s, char separator) {
            int i = 0, j = s.IndexOf(separator);
            while (j > i) {
                yield return new StringSegment(i, j - i);
                i = j + 1;
                j = s.IndexOf(separator, i);
            }
            int length = s.Length;
            if (i < length) {
                yield return new StringSegment(i, length - i);
            }
        }

        public static IEnumerable<StringSegment> GetSplit(this string s, char separator, StringSegment segment) {
            int i = segment.startIndex, j = s.IndexOf(separator, i);
            int lastIndex = segment.startIndex + segment.length;
            while (j > i && j < lastIndex) {
                yield return new StringSegment(i, j - i);
                i = j + 1;
                j = s.IndexOf(separator, i);
            }
            if (i < lastIndex) {
                yield return new StringSegment(i, lastIndex - i);
            }
        }

        public static int Count(this string s, char character) {
            int count = 0;
            int l = s.Length;
            for (int k = 0; k < l; k++) {
                if (s[k] == character) count++;
            }
            return count;
        }

        public static int Count(this string s, char character, StringSegment segment) {
            int count = 0;
            int l = segment.startIndex + segment.length;
            for (int k = segment.startIndex; k < l; k++) {
                if (s[k] == character) count++;
            }
            return count;
        }

        public static int GetSplitSegments(this string s, StringSegment segment, char separator, StringSegment[] segments) {
            int i = segment.startIndex;
            int lastIndex = i + segment.length;
            int count = segments.Length;
            for (int k = 0; k < count; k++) {
                int j = s.IndexOf(separator, i);
                if (j >= lastIndex) j = lastIndex;
                if (j > i) {
                    segments[k].startIndex = i;
                    segments[k].length = j - i;
                }
                i = j + 1;
                if (i >= lastIndex) return k + 1;
            }
            return count;
        }

        public static string GetSplitEntry(this string s, StringSegment segment) {
            return s.Substring(segment.startIndex, segment.length);
        }

    }
}
