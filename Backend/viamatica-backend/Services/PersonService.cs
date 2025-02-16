using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using viamatica_backend.DTOS;
using viamatica_backend.Models.Request;
using viamatica_backend.Models.Utility;
using viamatica_backend.Repository;

namespace viamatica_backend.Services
{
    public class PersonService
    {
        private readonly PersonaRepository _personRepository;
        private readonly IMapper _mapper;
        public PersonService(PersonaRepository personRepository, IMapper mapper)
        {
            _personRepository = personRepository;
            _mapper = mapper;
        }

        public async Task<APIResponse<PersonaDTO?>> UpdatePersonData(PersonaUpdateData data)
        {
            var persons = await _personRepository.GetFilteredAsync(e => e.IdPersona == data.IdPersona);
            var foundPerson = persons.FirstOrDefault();

            if (foundPerson == null)
            {
                return new APIResponse<PersonaDTO?>(null, "Persona no encontrada", HttpStatusCode.NotFound);
            }

            // 🔹 Actualizar manualmente los campos
            foundPerson.Nombres = data.Nombres ?? foundPerson.Nombres;
            foundPerson.Apellidos = data.Apellidos ?? foundPerson.Apellidos;

            // 🔹 Guardar los cambios en la base de datos
            var result = await _personRepository.UpdateAsync(foundPerson);

            // 🔹 Mapear a DTO antes de devolver la respuesta
            var responseDTO = _mapper.Map<PersonaDTO>(result);

            return new APIResponse<PersonaDTO?>(responseDTO, "Persona actualizada exitosamente", HttpStatusCode.OK);
        }
    }
}
