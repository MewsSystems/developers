using System.Collections.Generic;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Tests;

public static class TestData
{
    public const string CnbExchangeRateXml = """
                                             <kurzy banka="CNB" datum="29.07.2025" poradi="145">
                                             <tabulka typ="XML_TYP_CNB_KURZY_DEVIZOVEHO_TRHU">
                                             <radek kod="AUD" mena="dolar" mnozstvi="1" kurz="13,862" zeme="Austrálie"/>
                                             <radek kod="BRL" mena="real" mnozstvi="1" kurz="3,811" zeme="Brazílie"/>
                                             <radek kod="BGN" mena="lev" mnozstvi="1" kurz="12,582" zeme="Bulharsko"/>
                                             <radek kod="CNY" mena="žen-min-pi" mnozstvi="1" kurz="2,972" zeme="Čína"/>
                                             <radek kod="DKK" mena="koruna" mnozstvi="1" kurz="3,298" zeme="Dánsko"/>
                                             <radek kod="EUR" mena="euro" mnozstvi="1" kurz="24,610" zeme="EMU"/>
                                             <radek kod="PHP" mena="peso" mnozstvi="100" kurz="37,190" zeme="Filipíny"/>
                                             <radek kod="HKD" mena="dolar" mnozstvi="1" kurz="2,717" zeme="Hongkong"/>
                                             <radek kod="INR" mena="rupie" mnozstvi="100" kurz="24,563" zeme="Indie"/>
                                             <radek kod="IDR" mena="rupie" mnozstvi="1000" kurz="1,301" zeme="Indonesie"/>
                                             <radek kod="ISK" mena="koruna" mnozstvi="100" kurz="17,307" zeme="Island"/>
                                             <radek kod="ILS" mena="nový šekel" mnozstvi="1" kurz="6,335" zeme="Izrael"/>
                                             <radek kod="JPY" mena="jen" mnozstvi="100" kurz="14,344" zeme="Japonsko"/>
                                             <radek kod="ZAR" mena="rand" mnozstvi="1" kurz="1,187" zeme="Jižní Afrika"/>
                                             <radek kod="CAD" mena="dolar" mnozstvi="1" kurz="15,484" zeme="Kanada"/>
                                             <radek kod="KRW" mena="won" mnozstvi="100" kurz="1,530" zeme="Korejská republika"/>
                                             <radek kod="HUF" mena="forint" mnozstvi="100" kurz="6,153" zeme="Maďarsko"/>
                                             <radek kod="MYR" mena="ringgit" mnozstvi="1" kurz="5,038" zeme="Malajsie"/>
                                             <radek kod="MXN" mena="peso" mnozstvi="1" kurz="1,134" zeme="Mexiko"/>
                                             <radek kod="XDR" mena="ZPČ" mnozstvi="1" kurz="29,143" zeme="MMF"/>
                                             <radek kod="NOK" mena="koruna" mnozstvi="1" kurz="2,083" zeme="Norsko"/>
                                             <radek kod="NZD" mena="dolar" mnozstvi="1" kurz="12,687" zeme="Nový Zéland"/>
                                             <radek kod="PLN" mena="zlotý" mnozstvi="1" kurz="5,748" zeme="Polsko"/>
                                             <radek kod="RON" mena="leu" mnozstvi="1" kurz="4,848" zeme="Rumunsko"/>
                                             <radek kod="SGD" mena="dolar" mnozstvi="1" kurz="16,554" zeme="Singapur"/>
                                             <radek kod="SEK" mena="koruna" mnozstvi="1" kurz="2,205" zeme="Švédsko"/>
                                             <radek kod="CHF" mena="frank" mnozstvi="1" kurz="26,441" zeme="Švýcarsko"/>
                                             <radek kod="THB" mena="baht" mnozstvi="100" kurz="65,718" zeme="Thajsko"/>
                                             <radek kod="TRY" mena="lira" mnozstvi="100" kurz="52,573" zeme="Turecko"/>
                                             <radek kod="USD" mena="dolar" mnozstvi="1" kurz="21,331" zeme="USA"/>
                                             <radek kod="GBP" mena="libra" mnozstvi="1" kurz="28,455" zeme="Velká Británie"/>
                                             </tabulka>
                                             </kurzy>
                                             """;
    public const string InvalidCnbExchangeRateXml = """
                                             <kurzy banka="CNB" datum="29.07.2025" poradi="145">
                                             <tabulka typ="XML_TYP_CNB_KURZY_DEVIZOVEHO_TRHU">
                                             <radek kod="AUD" mena="dolar"  kurz="13,862" zeme="Austrálie"/>
                                             <radek kod="BRL" mena="real" mnozstvi="1"  zeme="Brazílie"/>
                                             </tabulka>
                                             </kurzy>
                                             """;

    public static string BaseCurrency = "CZK"; 
    public static readonly List<Currency> ValidCurrenciesForTest = [new Currency("USD"), new Currency("EUR")];
    public static readonly List<Currency> InvalidCurrenciesForTest = [new Currency("TEST")];
}