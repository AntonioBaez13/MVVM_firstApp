using MVVM_firstApp.ViewModels;
using System;
using System.Collections.Generic;

namespace MVVM_firstApp
{
    public interface IDatabaseOperations
    {
        (string trackPin, int trackTicketId) AddToDabataseAndPrint(IEnumerable<Combination> combinations, LoteriaViewModel selectedLoteria);
        string CreateRandomPin(int length);
        IEnumerable<LoteriaViewModel> GetAllLoterias();
        IEnumerable<Combination> GetCombinations(int ticketId);
        DateTimeOffset GetDateCreated(int ticketId);
        string GetLoteriaName(int ticketId);
        int GetSumOfValuesRepeated(DateTime date);
        List<TicketJugadaViewModel> GetTicketJugadaViewModel(DateTime date, string number, int loteriaId);
    }
}