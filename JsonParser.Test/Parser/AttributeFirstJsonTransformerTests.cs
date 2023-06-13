using System;
using System.IO;
using System.Linq;
using System.Text;
using JsonParser.Parser;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Tests
{
    public class AttributeFirstJsonTransformerTests
    {
        [Test]
        public void WhenEmptyObjectThenEmptyObject()
        {
            // Arrange
            var input = new MemoryStream(Encoding.ASCII.GetBytes("{}"));

            // Act
            var output = AttributeFirstJsonTransformer.Transform(input);
            var actual = new StreamReader(output).ReadToEnd();

            // Assert
            Assert.AreEqual("{}", actual);
        }

        [Test]
        public void WhenCorrectJsonThenCorrectResult()
        {
            var text = @"{""address"": {
                ""city"": ""New York"",
                ""state"": ""NY""
            },
            ""name"": ""John"",
            ""age"": 30 }";

            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sample1.json");
            var excepted = File.ReadAllText(filePath);

            var input = new MemoryStream(Encoding.ASCII.GetBytes(text));
            var output = AttributeFirstJsonTransformer.Transform(input);

            Assert.AreEqual(excepted, new StreamReader(output).ReadToEnd());
            //Assert.IsTrue(consoleOutput.ToString().Contains(actual));
        }

        [Test]
        public void WhenObjectWithPropertiesThenSetThem()
        {
            var text = @"{      
                 ""q1"": {
                     ""question"": ""5 + 7 = ?"",
                     ""options"": [
                     ""10"",
                     ""11"",
                     ""12"",
                     ""13""
                         ],
                     ""answer"": ""12""                 
             },
             ""Age"": 35}";
            var jsonObject = JToken.Parse(text);

            AttributeFirstJsonTransformer.ReorderProperties(jsonObject);
            
             Assert.AreEqual("answer",((JProperty)jsonObject.Last().Last().First()).Name);
             Assert.AreEqual("options",((JProperty)jsonObject.Last().Last().Last()).Name);
        }

        [Test]
        public void TestSimpleJsonFieldtoFail()
        {
            var simpleJson = @"{      
                 ""q1"": {
                     ""question"": ""5 + 7 = ?"",
                     ""options"": [
                     ""10"",
                     ""11"",
                     ""12"",
                     ""13""
                         ],
                     ""answer"": ""12""                 
             },
             ""Age"": 35}";
            var jobject = JToken.Parse(simpleJson);
            var firstObject = jobject.First();

            bool isComplexFiled = AttributeFirstJsonTransformer.IsComplexType(firstObject as JProperty);

            Assert.AreNotEqual(isComplexFiled, true);

        }
    }
}