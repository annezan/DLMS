using DLMS_MODELS.Bases;
using DLMS_MODELS.CommandeCompteurDomain.Responses;
using MediatR;
using System;
using System.Collections.Generic;

namespace DLMS_MODELS.CommandeCompteurDomain.Queries
{
    public class GetResultatCommandeCompteursByCommandeCompteurIdQuery : IRequest<ResponseBase<List<ResultatCommandeCompteurResponse>>>
    {
        public int CommandeCompteurId { get; }
        public Guid UserId { get; set; }

        public GetResultatCommandeCompteursByCommandeCompteurIdQuery(int commandeCompteurId)
        {
            CommandeCompteurId = commandeCompteurId;
        }
    }
}
