using DLMS_MODELS.Bases;
using DLMS_MODELS.CommandeCompteurDomain.Responses;
using MediatR;
using System;

namespace DLMS_MODELS.CommandeCompteurDomain.Queries
{
    public class GetResultatCommandeCompteurByIdQuery : IRequest<ResponseBase<ResultatCommandeCompteurResponse>>
    {
        public int Id { get; }
        public Guid UserId { get; set; }

        public GetResultatCommandeCompteurByIdQuery(int id)
        {
            Id = id;
        }
    }
}
