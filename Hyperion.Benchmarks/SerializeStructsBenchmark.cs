﻿#region copyright
// -----------------------------------------------------------------------
//  <copyright file="SerializeStructsBenchmark.cs" company="Akka.NET Team">
//      Copyright (C) 2015-2016 AsynkronIT <https://github.com/AsynkronIT>
//      Copyright (C) 2016-2016 Akka.NET Team <https://github.com/akkadotnet>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using System.IO;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;

namespace Hyperion.Benchmarks
{
    [Config(typeof(HyperionConfig))]
    public class SerializeStructsBenchmark
    {
        #region init
        private Serializer serializer;
        private MemoryStream stream;

        private StandardStruct standardValue;
        private BlittableStruct blittableValue;
        private TestEnum testEnum;

        [Setup]
        public void Setup()
        {
            serializer = new Serializer();
            stream = new MemoryStream();

            standardValue = new StandardStruct(1, "John", "Doe", isLoggedIn: false);
            blittableValue = new BlittableStruct(59, 92);
            testEnum = TestEnum.HatesAll;
        }

        [Cleanup]
        public void Cleanup()
        {
            stream.Dispose();
        }
        #endregion

        [Benchmark] public void Serialize_Enums() => serializer.Serialize(testEnum, stream);
        [Benchmark] public void Serialize_StandardValueTypes() => serializer.Serialize(standardValue, stream);
        [Benchmark] public void Serialize_BlittableValueTypes() => serializer.Serialize(blittableValue, stream);
    }

    #region test data types

    public struct StandardStruct
    {
        public readonly int Id;
        public readonly string FirstName;
        public readonly string LastName;
        public readonly bool IsLoggedIn;

        public StandardStruct(int id, string firstName, string lastName, bool isLoggedIn)
            : this()
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            IsLoggedIn = isLoggedIn;
        }
    }

    /// <summary>
    /// Blittable types have field layout matching their memory layout.
    /// See: https://docs.microsoft.com/en-us/dotnet/framework/interop/blittable-and-non-blittable-types
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct BlittableStruct
    {
        public readonly int X;
        public readonly int Y;

        public BlittableStruct(int x, int y) : this()
        {
            X = x;
            Y = y;
        }
    }

    public enum TestEnum
    {
        Married,
        Divorced,
        HatesAll
    }

    #endregion
}