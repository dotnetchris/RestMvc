using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace RestMvc.FunctionalTests
{
    [TestFixture]
    public class RoutingTests
    {
        private const string echoUri = "http://localhost/RestMvc/echo/hello";

        [Test]
        public void ShouldRouteToEchoAction()
        {
            var response = new HttpRequest("GET", echoUri).GetResponse();

            Assert.That(response.StatusCode, Is.EqualTo(200));
            Assert.That(response.Body, Is.EqualTo("hello"));
        }

        [Test]
        public void UnsupportedMethodShouldReturn405WithAllowHeaderSet()
        {
            var response = new HttpRequest("POST", echoUri).GetResponse();

            Assert.That(response.StatusCode, Is.EqualTo(405));
            Assert.That(response.Headers["Allow"], Is.EqualTo("GET"));
            Assert.That(response.Body, Is.EqualTo(""));
        }

        [Test]
        public void OptionsShouldSendAllowHeaderWithEmptyBody()
        {
            var response = new HttpRequest("OPTIONS", echoUri).GetResponse();

            Assert.That(response.StatusCode, Is.EqualTo(200));
            Assert.That(response.Headers["Allow"], Is.EqualTo("GET"));
            Assert.That(response.Body, Is.EqualTo(""));
        }

        [Test, Ignore("Content-length being set by IIS?")]
        public void HeadShouldSendBackSameHeadersAsGetWithEmptyBody()
        {
            var getResponse = new HttpRequest("GET", echoUri).GetResponse();
            var headResponse = new HttpRequest("HEAD", echoUri).GetResponse();

            Assert.That(headResponse.Body, Is.EqualTo(""));
            Assert.That(headResponse.Headers["Content-Length"], Is.EqualTo(getResponse.Headers["Content-Length"]));
        }
    }
}