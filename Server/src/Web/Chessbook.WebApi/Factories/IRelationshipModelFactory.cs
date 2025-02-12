﻿using Chessbook.Core.Domain.Relationships;
using Chessbook.Web.Models.Outputs;

namespace Chessbook.Web.Api.Factories
{
    public interface IRelationshipModelFactory
    {
        RelationshipDetailsModel PrepareRelationshipModel(Relationship relationship);
    }
}
