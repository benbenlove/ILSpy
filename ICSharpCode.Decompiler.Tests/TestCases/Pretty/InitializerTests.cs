// Copyright (c) AlphaSierraPapa for the SharpDevelop Team
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace ICSharpCode.Decompiler.Tests.TestCases.Pretty
{
	public class InitializerTests
	{
		#region Types and helpers
		public class C
		{
			public int Z;
			public S Y;
			public List<S> L;
		}

		public struct S
		{
			public int A;
			public int B;

			public S(int a)
			{
				A = a;
				B = 0;
			}
		}

		private enum MyEnum
		{
			a,
			b
		}

		private enum MyEnum2
		{
			c,
			d
		}

		private class Data
		{
			public List<MyEnum2> FieldList = new List<MyEnum2>();
			public MyEnum a {
				get;
				set;
			}
			public MyEnum b {
				get;
				set;
			}
			public List<MyEnum2> PropertyList {
				get;
				set;
			}

			public Data MoreData {
				get;
				set;
			}

			public StructData NestedStruct {
				get;
				set;
			}

			public Data this[int i] {
				get {
					return null;
				}
				set {
				}
			}

			public Data this[int i, string j] {
				get {
					return null;
				}
				set {
				}
			}

			public event EventHandler TestEvent;
		}

		private struct StructData
		{
			public int Field;
			public int Property {
				get;
				set;
			}

			public Data MoreData {
				get;
				set;
			}

			public StructData(int initialValue)
			{
				this = default(StructData);
				Field = initialValue;
				Property = initialValue;
			}
		}

		// Helper methods used to ensure initializers used within expressions work correctly
		private static void X(object a, object b)
		{
		}

		private static object Y()
		{
			return null;
		}

		public static void TestCall(int a, Thread thread)
		{

		}

		public static C TestCall(int a, C c)
		{
			return c;
		}
		#endregion

		public C Test1()
		{
			C c = new C();
			c.L = new List<S>();
			c.L.Add(new S(1));
			return c;
		}

		public C Test1Alternative()
		{
			return TestCall(1, new C {
				L = new List<S> {
					new S(1)
				}
			});
		}

		public C Test2()
		{
			C c = new C();
			c.Z = 1;
			c.Z = 2;
			return c;
		}

		public C Test3()
		{
			C c = new C();
			c.Y = new S(1);
			c.Y.A = 2;
			return c;
		}

		public C Test3b()
		{
			return TestCall(0, new C {
				Z = 1,
				Y = {
					A = 2
				}
			});
		}

		public C Test4()
		{
			C c = new C();
			c.Y.A = 1;
			c.Z = 2;
			c.Y.B = 3;
			return c;
		}


		public static void CollectionInitializerList()
		{
			X(Y(), new List<int> {
				1,
				2,
				3
			});
		}

		public static object RecursiveCollectionInitializer()
		{
			List<object> list = new List<object>();
			list.Add(list);
			return list;
		}

		public static void CollectionInitializerDictionary()
		{
			X(Y(), new Dictionary<string, int> {
			{
				"First",
				1
			},
			{
				"Second",
				2
			},
			{
				"Third",
				3
			}
		  });
		}

		public static void CollectionInitializerDictionaryWithEnumTypes()
		{
			X(Y(), new Dictionary<MyEnum, MyEnum2> {
			{
				MyEnum.a,
				MyEnum2.c
			},
			{
				MyEnum.b,
				MyEnum2.d
			}
		  });
		}

		public static void NotACollectionInitializer()
		{
			List<int> list = new List<int>();
			list.Add(1);
			list.Add(2);
			list.Add(3);
			X(Y(), list);
		}

		public static void ObjectInitializer()
		{
			X(Y(), new Data {
				a = MyEnum.a
			});
		}

		public static void NotAnObjectInitializer()
		{
			Data data = new Data();
			data.a = MyEnum.a;
			X(Y(), data);
		}

		public static void NotAnObjectInitializerWithEvent()
		{
			Data data = new Data();
			data.TestEvent += delegate {
				Console.WriteLine();
			};
			X(Y(), data);
		}

		public static void ObjectInitializerAssignCollectionToField()
		{
			X(Y(), new Data {
				a = MyEnum.a,
				FieldList = new List<MyEnum2> {
					MyEnum2.c,
					MyEnum2.d
				}
			});
		}

		public static void ObjectInitializerAddToCollectionInField()
		{
			X(Y(), new Data {
				a = MyEnum.a,
				FieldList = {
					MyEnum2.c,
					MyEnum2.d
				}
			});
		}

		public static void ObjectInitializerAssignCollectionToProperty()
		{
			X(Y(), new Data {
				a = MyEnum.a,
				PropertyList = new List<MyEnum2> {
					MyEnum2.c,
					MyEnum2.d
				}
			});
		}

		public static void ObjectInitializerAddToCollectionInProperty()
		{
			X(Y(), new Data {
				a = MyEnum.a,
				PropertyList = {
					MyEnum2.c,
					MyEnum2.d
				}
			});
		}

		public static void ObjectInitializerWithInitializationOfNestedObjects()
		{
			X(Y(), new Data {
				MoreData = {
					a = MyEnum.a,
					MoreData = {
						a = MyEnum.b
					}
				}
			});
		}

		private static int GetInt()
		{
			return 1;
		}

		private static string GetString()
		{
			return "Test";
		}

		private static void NoOp(Guid?[] array)
		{

		}

#if CS60
		public static void SimpleDictInitializer()
		{
			X(Y(), new Data {
				MoreData = {
					a = MyEnum.a,
					[2] = null
				}
			});
		}

		public static void MixedObjectAndDictInitializer()
		{
			X(Y(), new Data {
				MoreData = {
					a = MyEnum.a,
					[GetInt()] = {
						a = MyEnum.b,
						FieldList = {
							MyEnum2.c
						},
						[GetInt(), GetString()] = new Data(),
						[2] = null
					}
				}
			});
		}

		public void NestedListWithIndexInitializer()
		{
#if !OPT
			List<List<int>> list = new List<List<int>> {
#else
			List<List<int>> obj = new List<List<int>> {
#endif
				[0] = {
					1,
					2,
					3
				}
			};
		}
#endif

		public static void ObjectInitializerWithInitializationOfDeeplyNestedObjects()
		{
			X(Y(), new Data {
				a = MyEnum.b,
				MoreData = {
				  a = MyEnum.a,
				  MoreData = {
						MoreData = {
							MoreData = {
								MoreData = {
									MoreData = {
										MoreData = {
											a = MyEnum.b
										}
									}
								}
							}
						}
					}
			  }
			});
		}

		public static void CollectionInitializerInsideObjectInitializers()
		{
			X(Y(), new Data {
				MoreData = new Data {
					a = MyEnum.a,
					b = MyEnum.b,
					PropertyList = {
						MyEnum2.c
					}
				}
			});
		}

		public static void NotAStructInitializer_DefaultConstructor()
		{
			StructData structData = default(StructData);
			structData.Field = 1;
			structData.Property = 2;
			X(Y(), structData);
		}

		public static void StructInitializer_DefaultConstructor()
		{
			X(Y(), new StructData {
				Field = 1,
				Property = 2
			});
		}

		public static void NotAStructInitializer_ExplicitConstructor()
		{
			StructData structData = new StructData(0);
			structData.Field = 1;
			structData.Property = 2;
			X(Y(), structData);
		}

		public static void StructInitializer_ExplicitConstructor()
		{
			X(Y(), new StructData(0) {
				Field = 1,
				Property = 2
			});
		}

		public static void StructInitializerWithInitializationOfNestedObjects()
		{
			X(Y(), new StructData {
				MoreData = {
					a = MyEnum.a,
					FieldList = {
						MyEnum2.c,
						MyEnum2.d
					}
				}
			});
		}

		public static void StructInitializerWithinObjectInitializer()
		{
			X(Y(), new Data {
				NestedStruct = new StructData(2) {
					Field = 1,
					Property = 2
				}
			});
		}

		public static void Bug270_NestedInitialisers()
		{
			NumberFormatInfo[] source = null;

			TestCall(0, new Thread(Bug270_NestedInitialisers) {
				Priority = ThreadPriority.BelowNormal,
				CurrentCulture = new CultureInfo(0) {
					DateTimeFormat = new DateTimeFormatInfo {
						ShortDatePattern = "ddmmyy"
					},
					NumberFormat = (from format in source
									where format.CurrencySymbol == "$"
									select format).First()
				}
			});
		}

		public static void Bug953_MissingNullableSpecifierForArrayInitializer()
		{
			NoOp(new Guid?[1] {
				Guid.Empty
			});
		}
	}
}