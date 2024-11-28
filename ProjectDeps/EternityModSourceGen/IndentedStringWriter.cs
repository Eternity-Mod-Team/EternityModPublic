using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace JAssetsJourceGen
{
    ref struct IndentedStringWriter
    {
        public const string DefaultIndentString = "    ";
        public readonly StringBuilder Builder;
        public readonly string IndentString;
        public int Indent;

        private bool tabsPending;
        private void WriteTabs()
        {
            if (tabsPending)
            {
                for (int i = 0; i < Indent; i++)
                    Builder.Append(IndentString);
                tabsPending = false;
            }
        }
        //public IndentedStringWriter()
        //{
        //    Builder = new StringBuilder();
        //    Indent = 0;
        //    IndentString = DefaultIndentString;
        //    tabsPending = false;
        //}
        public IndentedStringWriter(int capacity)
        {
            Builder = new StringBuilder(capacity);
            Indent = 0;
            IndentString = DefaultIndentString;
            tabsPending = false;
        }
        public IndentedStringWriter(StringBuilder builder)
        {
            Builder = builder;
            Indent = 0;
            IndentString = DefaultIndentString;
            tabsPending = false;
        }

        #region Write methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IndentedStringWriter Write(byte value)
        {
            if (tabsPending)
                WriteTabs();
            Builder.Append(value);
            return this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IndentedStringWriter Write(sbyte value)
        {
            if (tabsPending)
                WriteTabs();
            Builder.Append(value);
            return this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IndentedStringWriter Write(short value)
        {
            if (tabsPending)
                WriteTabs();
            Builder.Append(value);
            return this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IndentedStringWriter Write(ushort value)
        {
            if (tabsPending)
                WriteTabs();
            Builder.Append(value);
            return this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IndentedStringWriter Write(int value)
        {
            if (tabsPending)
                WriteTabs();
            Builder.Append(value);
            return this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IndentedStringWriter Write(uint value)
        {
            if (tabsPending)
                WriteTabs();
            Builder.Append(value);
            return this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IndentedStringWriter Write(long value)
        {
            if (tabsPending)
                WriteTabs();
            Builder.Append(value);
            return this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IndentedStringWriter Write(ulong value)
        {
            if (tabsPending)
                WriteTabs();
            Builder.Append(value);
            return this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IndentedStringWriter Write(float value)
        {
            if (tabsPending)
                WriteTabs();
            Builder.Append(value);
            return this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IndentedStringWriter Write(double value)
        {
            if (tabsPending)
                WriteTabs();
            Builder.Append(value);
            return this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IndentedStringWriter Write(char value)
        {
            if (tabsPending)
                WriteTabs();
            Builder.Append(value);
            return this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IndentedStringWriter Write(string value)
        {
            if (tabsPending)
                WriteTabs();
            Builder.Append(value);
            return this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IndentedStringWriter Write(string value,int start, int count)
        {
            if (tabsPending)
                WriteTabs();
            Builder.Append(value, start, count);
            return this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IndentedStringWriter Write(ReadOnlySpan<char> values)
        {
            if (tabsPending)
                WriteTabs();
            //fixed (char* c = values)
            foreach(char c in values)
                Builder.Append(c);
            return this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IndentedStringWriter Write([InterpolatedStringHandlerArgument("")] ref IndentedStringWriterInterpolatedStringHandler handler)
        {
            tabsPending = false;
            return this;
        }
        #endregion // Write methods

        #region WriteLine methods
        /*
        public readonly IndentedStringWriter WriteLine(byte value)
        {
            Builder.Append(value).AppendLine();
            return this;
        }
        public readonly IndentedStringWriter WriteLine(sbyte value)
        {
            Builder.Append(value).AppendLine();
            return this;
        }
        public readonly IndentedStringWriter WriteLine(short value)
        {
            Builder.Append(value).AppendLine();
            return this;
        }
        public readonly IndentedStringWriter WriteLine(ushort value)
        {
            Builder.Append(value).AppendLine();
            return this;
        }
        public readonly IndentedStringWriter WriteLine(int value)
        {
            Builder.Append(value).AppendLine();
            return this;
        }
        public readonly IndentedStringWriter WriteLine(uint value)
        {
            Builder.Append(value).AppendLine();
            return this;
        }
        public readonly IndentedStringWriter WriteLine(long value)
        {
            Builder.Append(value).AppendLine();
            return this;
        }
        public readonly IndentedStringWriter WriteLine(ulong value)
        {
            Builder.Append(value).AppendLine();
            return this;
        }
        public readonly IndentedStringWriter WriteLine(float value)
        {
            Builder.Append(value).AppendLine();
            return this;
        }
        public readonly IndentedStringWriter WriteLine(double value)
        {
            Builder.Append(value).AppendLine();
            return this;
        }
        public readonly IndentedStringWriter WriteLine(char value)
        {
            Builder.Append(value).AppendLine();
            return this;
        }
        public readonly IndentedStringWriter WriteLine(string value)
        {
            Builder.AppendLine(value);
            return this;
        }
        public unsafe readonly IndentedStringWriter WriteLine(ReadOnlySpan<char> values)
        {
            fixed (char* c = values)
                Builder.Append(c, values.Length);
            return this;
        }*/
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IndentedStringWriter WriteLine()
        {
            if (tabsPending)
                WriteTabs();
            Builder.AppendLine();
            tabsPending = true;
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IndentedStringWriter WriteLine(char value)
        {
            if (tabsPending)
                WriteTabs();
            Builder.Append(value).AppendLine();
            tabsPending = true;
            return this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IndentedStringWriter WriteLine(string str)
        {
            if (tabsPending)
                WriteTabs();
            Builder.AppendLine(str);
            tabsPending = true;
            return this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IndentedStringWriter WriteLine(string str, int start, int count)
        {
            if (tabsPending)
                WriteTabs();
            Builder.Append(str, start, count).AppendLine();
            tabsPending = true;
            return this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IndentedStringWriter WriteLine([InterpolatedStringHandlerArgument("")] ref IndentedStringWriterInterpolatedStringHandler handler)
        {
            if (tabsPending)
                WriteTabs();
            WriteLine();
            return this;
        }
        #endregion

        internal string ToStringAndClear()
        {
            string result = Builder.ToString();
            Builder.Clear();
            Indent = 0;
            tabsPending = true;
            return result;
        }
        internal void BeginScope()
        {
            WriteLine();
            WriteLine('{');
            Indent++;
        }
        internal void BeginScope(string text)
        {
            WriteLine(text);
            WriteLine('{');
            Indent++;
        }
        internal void BeginScope([InterpolatedStringHandlerArgument("")] ref IndentedStringWriterInterpolatedStringHandler handler)
        {
            WriteLine();
            WriteLine('{');
            Indent++;
        }
        internal void EndScope()
        {
            Indent--;
            WriteLine('}');
        }
    }

    [InterpolatedStringHandler]
    internal ref struct IndentedStringWriterInterpolatedStringHandler
    {
        // this writes only one line so the state on the writer doesnt really change
        private IndentedStringWriter writer;

        public IndentedStringWriterInterpolatedStringHandler(int literalLength, int formattedCount, IndentedStringWriter writer)
        {
            this.writer = writer;
            writer.Builder.EnsureCapacity(writer.Builder.Capacity + literalLength + formattedCount * 2);
        }
        public void AppendFormatted(int value)
        {
            writer.Write(value);
        }
        public void AppendFormatted(string? value)
        {
            if (value != null)
                writer.Write(value);
        }
        //public void AppendFormatted<T>(T value)
        //{
        //    if (value != null)
        //        writer.Write(value.ToString());
        //}
        public void AppendLiteral(string s)
        {
            writer.Write(s);
        }
    }

}
