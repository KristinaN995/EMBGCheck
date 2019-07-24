using System;
using System.Globalization;
using System.Linq;
using System.Web.Services;
/// <summary>
/// Summary description for WebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class WebService : System.Web.Services.WebService
{
    ///<summary>
    /// Web Method for Ckecking EMBG
    /// Gets the EMBG
    /// Checks if it's valid:day,month,year,gender,city code,CRC
    ///</summary>
    ///<returns>
    ///Returns true if the EMBG is correct and false if it's a not valid EMBG
    ///valid for North Macedonia
    ///</returns>
    ///<include file='docs.xml' path='docs/members[@name="WebService"]/CheckEmbg/*'/>
    [WebMethod]
    public bool CheckEmbg(string embg)
    {
        bool embgcheck = false;
        if (embg.Length == 13)
            if (embg.All(char.IsDigit))
                 if (CheckDate(embg))
                    if (Enumerable.Range(41,9).Contains(Int32.Parse(embg.Substring(7, 2)))) //validno mesto
                       if(CRCKalkulacija(embg) == Int32.Parse(embg.Substring(embg.Length - 1, 1))) //crc pominuva
                        embgcheck = true;
        return embgcheck;
    }
    ///<summary>
    /// Checking the date
    /// Checks if it's valid:day,month,year
    ///</summary>
    ///<returns>
    ///Returns true if it's an existing date and false if it's not
    ///</returns>
    ///<include file='docs.xml' path='docs/members[@name="WebService"]/CheckDate/*'/>
    private bool CheckDate(string embg)
    {
        DateTime temp;
            DateTime.TryParseExact(embg.Substring(0, 4) + (embg.Substring(4, 1) == "0" ? "2" : "1") + embg.Substring(4, 3),
             "ddMMyyyy", null, DateTimeStyles.None, out temp);
        if (temp != DateTime.MinValue && temp <= DateTime.Today && temp.Year>=DateTime.Today.Year-120)
            return true;
        else return false;
    }
    ///<summary>
    /// Calculating the CRC
    ///</summary>
    ///<returns>
    ///Returns the calculated CRC
    ///</returns>
    ///<include file='docs.xml' path='docs/members[@name="WebService"]/CRCKalkulacija/*'/>
    private int CRCKalkulacija(string embg)
    {
        int kpom = 0;
        for (int i = 0; i < 6; i++)        
            kpom = ((7 - i) * (Int32.Parse(embg.Substring(i, 1)) + Int32.Parse(embg.Substring(i + 6, 1)))) + kpom;        
        kpom = 11 - (kpom % 11);
        if (kpom >9)
            kpom = 0;
        return kpom;
    }
}
