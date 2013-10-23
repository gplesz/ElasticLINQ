﻿// Copyright (c) Tier 3 Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 

using ElasticLinq;
using ElasticLinq.Mapping;
using ElasticLinq.Response.Model;
using ElasticLINQ.Test.TestSupport;
using System;
using System.Reflection;
using Xunit;

namespace ElasticLINQ.Test.Mapping
{
    public class ElasticFieldsMappingWrapperTests
    {
        [Fact]
        public void GetTypeNamePassesThroughToUnderlingMapping()
        {
            var fakeMapping = new FakeElasticMapping();
            var mapping = new ElasticFieldsMappingWrapper(fakeMapping);
            var type = typeof(ElasticFieldsMappingWrapperTests);

            mapping.GetTypeName(type);

            Assert.Single(fakeMapping.GetTypeNames, type);
        }

        [Fact]
        public void GetObjectSourcesPassesThroughToUnderlingMapping()
        {
            var fakeMapping = new FakeElasticMapping();
            var mapping = new ElasticFieldsMappingWrapper(fakeMapping);
            var type = typeof(ElasticFieldsMappingWrapperTests);
            var hit = new Hit { _id = "testing" };

            mapping.GetObjectSource(type, hit);

            Assert.Single(fakeMapping.GetObjectSources, Tuple.Create(type, hit));
        }

        [Fact]
        public void GetFieldNamePassesThroughToUnderlingMappingWhenNotElasticField()
        {
            var fakeMapping = new FakeElasticMapping();
            var mapping = new ElasticFieldsMappingWrapper(fakeMapping);
            var memberInfo = MethodBase.GetCurrentMethod();

            mapping.GetFieldName(memberInfo);

            Assert.Single(fakeMapping.GetFieldNames, memberInfo);
        }

        [Fact]
        public void GetFieldNameReturnsScoreForElasticFieldScore()
        {
            var fakeMapping = new FakeElasticMapping();
            var mapping = new ElasticFieldsMappingWrapper(fakeMapping);
            var memberInfo = typeof(ElasticFields).GetMember("Score")[0];

            var fieldName = mapping.GetFieldName(memberInfo);

            Assert.Equal("_score", fieldName);
            Assert.Empty(fakeMapping.GetFieldNames);
        }

    }
}