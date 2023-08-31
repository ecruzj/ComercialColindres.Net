using ComercialColindres.Datos.Entorno.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ComercialColindres.ReglasNegocio.DomainServices
{
    public class BoletaHumedadAsignacionDomainServices : IBoletaHumedadAsignacionDomainServices
    {
        public void TryAssignBoletaHumidityToBoleta(Boletas boleta, BoletaHumedad boletaHumedad)
        {
            if (boletaHumedad == null) throw new ArgumentNullException(nameof(boletaHumedad));

            if (boleta == null || boletaHumedad.BoletaHumedadAsignacion != null) return;

            BoletaHumedadAsignacion boletaHumidityAssignment = new BoletaHumedadAsignacion(boleta, boletaHumedad);
            boleta.AddBoletaHumidityAssignment(boletaHumidityAssignment);
        }

        public void RemoveBoletasHumidityWithPaymentFromOthers(List<BoletaHumedadAsignacion> boletasHumidityAssignment, int boletaId)
        {
            if (boletasHumidityAssignment == null) throw new ArgumentNullException(nameof(boletasHumidityAssignment));

            List<BoletaHumedadAsignacion> humidityAssignment = boletasHumidityAssignment.ToList();
            foreach (BoletaHumedadAsignacion asignacion in humidityAssignment)
            {
                if (asignacion.BoletaHumedad.BoletaHumedadPago == null) continue;

                //Si la BoletaHumedad fue asignada automaticamente, no debe eliminarlo
                //solo puede eliminar pagos de boletas con humedad asignados manualmente.
                if (asignacion.BoletaHumedad.BoletaHumedadPago.BoletaId != boletaId || asignacion.Boleta.BoletaId == boletaId)
                {
                    boletasHumidityAssignment.Remove(asignacion);
                }
            }
        }
    }
}
