using System;
using System.Collections.Immutable;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Language.Intellisense.AsyncCompletion;
using Microsoft.VisualStudio.Language.Intellisense.AsyncCompletion.Data;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace HelloWorldCompletion
{
    [Export(typeof(IAsyncCompletionSourceProvider))]
    [ContentType("CSharp")]
    [Name("Hello World completion item source")]
    internal class HelloWorldCompletionSourceProvider : IAsyncCompletionSourceProvider
    {
        Lazy<HelloWorldCompletionSource> Source = new Lazy<HelloWorldCompletionSource>(() => new HelloWorldCompletionSource());

        public IAsyncCompletionSource GetOrCreate(ITextView textView)
        {
            return Source.Value;
        }
    }

    public class HelloWorldCompletionSource : IAsyncCompletionSource
    {
        public static bool ShouldReturnItems { get; set; } = true;
        public static bool ShouldDismiss { get; set; } = false;
        public static double Delay { get; set; } = 0;

        private ImmutableArray<CompletionItem> sampleItemsOnEvenLine;
        private ImmutableArray<CompletionItem> sampleItemsOnOddLine;

        public HelloWorldCompletionSource()
        {
            sampleItemsOnEvenLine = ImmutableArray.Create(
                new CompletionItem("Hello", this),
                new CompletionItem("World", this),
                new CompletionItem("even", this)
            );
            sampleItemsOnOddLine = ImmutableArray.Create(
                new CompletionItem("Hello", this),
                new CompletionItem("World", this),
                new CompletionItem("odd", this)
            );
        }

        public async Task<CompletionContext> GetCompletionContextAsync(IAsyncCompletionSession session, CompletionTrigger trigger, SnapshotPoint triggerLocation, SnapshotSpan applicableToSpan, CancellationToken token)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(Delay));

            if (ShouldDismiss)
            {
                session.Dismiss();
            }

            if (ShouldReturnItems)
            {
                return new CompletionContext(
                    triggerLocation.GetContainingLine().LineNumber % 2 == 0
                    ? sampleItemsOnEvenLine
                    : sampleItemsOnOddLine);
            }
            else
            {
                return CompletionContext.Empty;
            }
        }

        public async Task<object> GetDescriptionAsync(IAsyncCompletionSession session, CompletionItem item, CancellationToken token)
        {
            switch (item.DisplayText)
            {
                case "even":
                    return "Hello! We are on an even line number";
                case "odd":
                    return "Hello! We are on an odd line number";
                default:
                    return "Hello! This is a sample item.";
            }
        }

        public CompletionStartData InitializeCompletion(CompletionTrigger trigger, SnapshotPoint triggerLocation, CancellationToken token)
        {
            // Since we are plugging in to CSharp content type,
            // allow the CSharp language service to pick the Applicable To Span.
            return CompletionStartData.ParticipatesInCompletionIfAny;
        }
    }
}
