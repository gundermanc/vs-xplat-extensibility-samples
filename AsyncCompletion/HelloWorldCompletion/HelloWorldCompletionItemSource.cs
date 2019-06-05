using System;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Core.Imaging;
using Microsoft.VisualStudio.Language.Intellisense.AsyncCompletion;
using Microsoft.VisualStudio.Language.Intellisense.AsyncCompletion.Data;
using Microsoft.VisualStudio.Language.StandardClassification;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;

namespace HelloWorldCompletion
{
    public class HelloWorldCompletionSource : IAsyncCompletionSource
    {
        public static bool ShouldReturnItems { get; set; } = true;
        public static bool ShouldDismiss { get; set; } = false;
        public static double Delay { get; set; } = 0;

        private ImmutableArray<CompletionItem> sampleItemsOnEvenLine;
        private ImmutableArray<CompletionItem> sampleItemsOnOddLine;
        static ImageElement CompletionItemIcon = new ImageElement(new ImageId(new Guid("ae27a6b0-e345-4288-96df-5eaf394ee369"), 3533), "Unknown");

        public HelloWorldCompletionSource()
        {
            sampleItemsOnEvenLine = ImmutableArray.Create(
                new CompletionItem("Hello", this, CompletionItemIcon),
                new CompletionItem("World", this, CompletionItemIcon),
                new CompletionItem("even", this, CompletionItemIcon)
            );
            sampleItemsOnOddLine = ImmutableArray.Create(
                new CompletionItem("Hello", this, CompletionItemIcon),
                new CompletionItem("World", this, CompletionItemIcon),
                new CompletionItem("odd", this, CompletionItemIcon)
            );
        }

        public CompletionStartData InitializeCompletion(CompletionTrigger trigger, SnapshotPoint triggerLocation, CancellationToken token)
        {
            // Since we are plugging in to CSharp content type,
            // allow the CSharp language service to pick the Applicable To Span.
            return CompletionStartData.ParticipatesInCompletionIfAny;
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
                    triggerLocation.GetContainingLine().LineNumber % 2 == 1 // arrays start at 0, so convert to human readable format
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
            var message = string.Empty;
            switch (item.DisplayText)
            {
                case "even":
                    message = "This IntelliSense completion is on an even line number";
                    break;
                case "odd":
                    message = "This IntelliSense completion is on an odd line number";
                    break;
                default:
                    message = "This is a sample item.";
                    break;
            }

            var content = new ContainerElement(
                ContainerElementStyle.Wrapped,
                CompletionItemIcon,
                new ClassifiedTextElement(
                    new ClassifiedTextRun(PredefinedClassificationTypeNames.Keyword, "Hello! "),
                    new ClassifiedTextRun(PredefinedClassificationTypeNames.Identifier, message)));

            var contentContainer = new ContainerElement(
                ContainerElementStyle.Stacked,
                content,
                new ClassifiedTextElement(
                    new ClassifiedTextRun(
                        PredefinedClassificationTypeNames.Identifier,
                        "The current date and time is: " + DateTime.Now.ToString())));

            return contentContainer;
        }
    }
}
