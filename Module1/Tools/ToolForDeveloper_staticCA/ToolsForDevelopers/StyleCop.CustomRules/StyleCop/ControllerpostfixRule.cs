using System.Web.Mvc;
using StyleCop;
using StyleCop.CSharp;

namespace StaticCodeAnalyzer.StyleCop
{
    [SourceAnalyzer(typeof(CsParser))]
    public class ControllerpostfixRule : SourceAnalyzer
    {
        const string ruleName = "ControllerpostfixRule";
        public override void AnalyzeDocument(CodeDocument document)
        {
            CsDocument csDocument = (CsDocument)document;
            csDocument.WalkDocument(new CodeWalkerElementVisitor<object>(this.VisitElement));
        }
        private bool VisitElement(object element, CsElement parentElement, object context)
        {

            var controllerElement = element as Controller;
            if(controllerElement != null)
            {
                if (!controllerElement.ControllerContext.RouteData.Values["controller"].ToString().EndsWith("Controller", System.StringComparison.Ordinal))
                    this.AddViolation((ICodeElement)element, ruleName);
                return false;
            }
            return true;

        }
      
    }
}