﻿using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

/// <summary>
/// Summary description for NumberToEnglish
/// </summary>
public class NumberToEnglish
{
	public NumberToEnglish()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public String changeNumericToWords(decimal numb)
    {

        String num = numb.ToString();
        return changeToWords(num, false);

    }

    public String changeCurrencyToWords(String numb)
    {

        return changeToWords(numb, true);

    }

    public String changeNumericToWords(String numb)
    {

        return changeToWords(numb, false);

    }

    //public String changeCurrencyToWords(double numb)
    //{

    //    return changeToWords(numb.ToString(), true);

    //}

    private String changeToWords(String numb, bool isCurrency)
    {

        String val = "", wholeNo = numb, points = "", andStr = "", pointStr = "";

        String endStr = (isCurrency) ? ("Only") : ("");

        try
        {

            int decimalPlace = numb.IndexOf(".");

            if (decimalPlace > 0)
            {

                wholeNo = numb.Substring(0, decimalPlace);

                points = numb.Substring(decimalPlace + 1);

                if (Convert.ToInt32(points) > 0)
                {

                    andStr = (isCurrency) ? ("and") : ("and Paisa ");// just to separate whole numbers from points/cents

                    endStr = (isCurrency) ? ("Cents " + endStr) : ("Only");

                    pointStr = translateWholeNumber(points);

                }

            }

            val = String.Format("{0} {1}{2} {3}", translateWholeNumber(wholeNo).Trim(), andStr, pointStr, endStr);

        }

        catch { ;}

        return val;

    }

    private String translateWholeNumber(String number)
    {

        string word = "";

        try
        {

            bool beginsZero = false;//tests for 0XX

            bool isDone = false;//test if already translated

            double dblAmt = (Convert.ToDouble(number));

            //if ((dblAmt > 0) && number.StartsWith("0"))

            if (dblAmt > 0)
            {//test for zero or digit zero in a nuemric

                beginsZero = number.StartsWith("0");

                number = dblAmt.ToString();
                int numDigits = number.Length;

                int pos = 0;//store digit grouping

                String place = "";//digit grouping name:hundres,thousand,etc...

                switch (numDigits)
                {

                    case 1://ones' range

                        word = ones(number);

                        isDone = true;

                        break;

                    case 2://tens' range

                        word = tens(number);

                        isDone = true;

                        break;

                    case 3://hundreds' range

                        pos = (numDigits % 3) + 1;

                        place = " Hundred ";

                        break;

                    case 4://thousands' range

                    case 5:
                        pos = (numDigits % 4) + 1;

                        place = " Thousand ";

                        break;
                    case 6://Lacs' range  

                    case 7:
                        pos = (numDigits % 6) + 1;

                        place = " Lac ";

                        break;
                    case 8://Crores' range
                    case 9:
                    case 10:
                    case 11:
                    case 12:

                        pos = (numDigits % 8) + 1;

                        place = " Crore ";

                        break;

                    default:

                        isDone = true;

                        break;

                }

                if (!isDone)
                {//if transalation is not done, continue...(Recursion comes in now!!)

                    word = translateWholeNumber(number.Substring(0, pos)) + place + translateWholeNumber(number.Substring(pos));

                    //check for trailing zeros

                    if (beginsZero) word = " " + word.Trim();

                }

                //ignore digit grouping names

                if (word.Trim().Equals(place.Trim())) word = "";

            }

        }

        catch { ;}

        return word.Trim();

    }

    private String tens(String digit)
    {

        int digt = Convert.ToInt32(digit);

        String name = null;

        switch (digt)
        {

            case 10:

                name = "Ten";

                break;

            case 11:

                name = "Eleven";

                break;

            case 12:

                name = "Twelve";

                break;

            case 13:

                name = "Thirteen";

                break;

            case 14:

                name = "Fourteen";

                break;

            case 15:

                name = "Fifteen";

                break;

            case 16:

                name = "Sixteen";

                break;

            case 17:

                name = "Seventeen";

                break;

            case 18:

                name = "Eighteen";

                break;

            case 19:

                name = "Nineteen";

                break;

            case 20:

                name = "Twenty";

                break;

            case 30:

                name = "Thirty";

                break;

            case 40:

                name = "Fourty";

                break;

            case 50:

                name = "Fifty";

                break;

            case 60:

                name = "Sixty";

                break;

            case 70:

                name = "Seventy";

                break;

            case 80:

                name = "Eighty";

                break;

            case 90:

                name = "Ninety";

                break;

            default:

                if (digt > 0)
                {

                    name = tens(digit.Substring(0, 1) + "0") + " " + ones(digit.Substring(1));

                }

                break;

        }

        return name;

    }

    private String ones(String digit)
    {

        int digt = Convert.ToInt32(digit);

        String name = "";

        switch (digt)
        {

            case 1:

                name = "One";

                break;

            case 2:

                name = "Two";

                break;

            case 3:

                name = "Three";

                break;

            case 4:

                name = "Four";

                break;

            case 5:

                name = "Five";

                break;

            case 6:

                name = "Six";

                break;

            case 7:

                name = "Seven";

                break;

            case 8:

                name = "Eight";

                break;

            case 9:

                name = "Nine";

                break;

        }

        return name;

    }
}
