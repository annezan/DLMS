using DLMS_MODELS.Bases;
using DLMS_MODELS.CommandeCompteurDomain.Responses;
using MediatR;
using System;
using System.Collections.Generic;

namespace DLMS_MODELS.CommandeCompteurDomain.Queries
{
    public class GetResultatCommandeCompteursQuery : IRequest<ResponseBase<List<ResultatCommandeCompteurResponse>>>
    {
        public Guid UserId { get; set; }

        public GetResultatCommandeCompteursQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
