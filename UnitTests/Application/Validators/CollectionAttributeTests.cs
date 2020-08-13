using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Application.Validators;
using NUnit.Framework;

namespace Tests.Application.Validators
{
    public static class M
    {
        public static string a = "";
    }

    public class CollectionAttributeTests
    {
        public static List<string> NullCollection = null;
        public static List<string> EmptyCollection = new List<string>();
        // Code convention L{Length of collection}E{Length of collection}Collection
        public static List<string> L1E0Collection = new List<string> {""};
        public static List<string> L2E0Collection = new List<string> {"", ""};
        public static List<string> L1E1Collection = new List<string> {"1"};
        public static List<string> L2E1Collection = new List<string> {"1", "2"};
        public static List<string> L1E2Collection = new List<string> {"11"};
        public static List<string> L2E2Collection = new List<string> {"11", "22"};


        public static object[] InvalidMinLengthCases = {
            new object[] {NullCollection, nameof(TestClass.ML0MEL0)},

            new object[] {NullCollection, nameof(TestClass.ML0MEL2)},

            new object[] {NullCollection, nameof(TestClass.ML2MEL0)},
            new object[] {EmptyCollection, nameof(TestClass.ML2MEL0)},
            new object[] {L1E0Collection, nameof(TestClass.ML2MEL0)},
            new object[] {L1E1Collection, nameof(TestClass.ML2MEL0)},
            new object[] {L1E2Collection, nameof(TestClass.ML2MEL0)},

            new object[] {NullCollection, nameof(TestClass.ML2MEL2)},
            new object[] {EmptyCollection, nameof(TestClass.ML2MEL2)},
            new object[] {L1E0Collection, nameof(TestClass.ML2MEL2)},
            new object[] {L1E1Collection, nameof(TestClass.ML2MEL2)},
            new object[] {L1E2Collection, nameof(TestClass.ML2MEL2)}
        };

        public static object[] ValidMinLengthCases = {
            new object[] {EmptyCollection, nameof(TestClass.ML0MEL0)},
            new object[] {L1E0Collection, nameof(TestClass.ML0MEL0)},
            new object[] {L2E0Collection, nameof(TestClass.ML0MEL0)},
            new object[] {L1E1Collection, nameof(TestClass.ML0MEL0)},
            new object[] {L2E1Collection, nameof(TestClass.ML0MEL0)},
            new object[] {L1E2Collection, nameof(TestClass.ML0MEL0)},
            new object[] {L2E2Collection, nameof(TestClass.ML0MEL0)},

            new object[] {L1E2Collection, nameof(TestClass.ML0MEL2)},
            new object[] {L2E2Collection, nameof(TestClass.ML0MEL2)},

            new object[] {L2E0Collection, nameof(TestClass.ML2MEL0)},
            new object[] {L2E1Collection, nameof(TestClass.ML2MEL0)},
            new object[] {L2E2Collection, nameof(TestClass.ML2MEL0)},

            new object[] {L2E2Collection, nameof(TestClass.ML2MEL2)}
        };

        public static object[] InvalidMinElementLengthCases = {

            new object[] {L1E0Collection, nameof(TestClass.ML0MEL2)},
            new object[] {L2E0Collection, nameof(TestClass.ML0MEL2)},
            new object[] {L1E1Collection, nameof(TestClass.ML0MEL2)},
            new object[] {L2E1Collection, nameof(TestClass.ML0MEL2)},

            new object[] {L2E0Collection, nameof(TestClass.ML2MEL2)},
            new object[] {L2E1Collection, nameof(TestClass.ML2MEL2)}
        };

        public static object[] ValidMinElementLengthCases = {
            new object[] {EmptyCollection, nameof(TestClass.ML0MEL0)},
            new object[] {L1E0Collection, nameof(TestClass.ML0MEL0)},
            new object[] {L2E0Collection, nameof(TestClass.ML0MEL0)},

            new object[] {L1E2Collection, nameof(TestClass.ML0MEL2)},
            new object[] {L2E2Collection, nameof(TestClass.ML0MEL2)},

            new object[] {L2E0Collection, nameof(TestClass.ML2MEL0)},
            new object[] {L2E1Collection, nameof(TestClass.ML2MEL0)},
            new object[] {L2E2Collection, nameof(TestClass.ML2MEL0)},

            new object[] {L2E2Collection, nameof(TestClass.ML2MEL2)}
        };

        [Test]
        public void NullCollectionInvalid()
        {
            Assert.Throws<ValidationException>(() => Validate(NullCollection, nameof(TestClass.ML0MEL0)));
            Assert.Throws<ValidationException>(() => Validate(NullCollection, nameof(TestClass.ML0MEL2)));
            Assert.Throws<ValidationException>(() => Validate(NullCollection, nameof(TestClass.ML2MEL0)));
            Assert.Throws<ValidationException>(() => Validate(NullCollection, nameof(TestClass.ML2MEL2)));
        }

        [Test, TestCaseSource(nameof(InvalidMinLengthCases))]
        public void CollectionLengthLessThanMinLengthTest(ICollection<string> testValue, string collectionName)
        {
            Assert.Throws<ValidationException>(() => Validate(testValue, collectionName));
        }

        [Test, TestCaseSource(nameof(InvalidMinElementLengthCases))]
        public void CollectionElementLengthLessThanMinElementLengthTest(ICollection<string> testValue, string collectionName)
        {
            Assert.Throws<ValidationException>(() => Validate(testValue, collectionName));
        }

        [Test, TestCaseSource(nameof(ValidMinLengthCases))]
        public void CollectionLengthGreaterThanMinLengthTest(ICollection<string> testValue, string collectionName)
        {
            Assert.DoesNotThrow(() => Validate(testValue, collectionName));
        }

        [Test, TestCaseSource(nameof(ValidMinElementLengthCases))]
        public void CollectionElementLengthGreaterThanMinElementLengthTest(ICollection<string> testValue, string collectionName)
        {
            Assert.DoesNotThrow(() => Validate(testValue, collectionName));
        }

        private static void Validate(ICollection<string> testValue, string collectionName)
        {
            var testType = typeof(TestClass)
                .GetProperty(collectionName)?
                .GetCustomAttribute(typeof(CollectionAttribute)) as CollectionAttribute;

            testType?.Validate(testValue, collectionName);
        }
    }

    public class TestClass
    {
        [Collection]
        public ICollection<string> ML0MEL0 { get; set; }

        [Collection(MinLength = 2)]
        public ICollection<string> ML2MEL0 { get; set; }

        [Collection(MinElementLength = 2)]
        public ICollection<string> ML0MEL2 { get; set; }

        [Collection(MinLength = 2, MinElementLength = 2)]
        public ICollection<string> ML2MEL2 { get; set; }
    }
}