using System;

public class StrongPasswordChecker
{
    public static int MinimumChanges(string password)
    {
        int changes = 0;
        bool hasLowercase = false, hasUppercase = false, hasDigit = false;
        int repeatCount = 0, repeatLength = 0;
        
        for (int i = 0; i < password.Length; i++)
        {
            // for every letter in the password we check wether all the conditions
            // are met (at least one of: digit, uppercase letter, lowercase letter
            if (char.IsLower(password[i])) 
                hasLowercase = true;
            else if (char.IsUpper(password[i])) 
                hasUppercase = true;
            else if (char.IsDigit(password[i])) 
                hasDigit = true;

            // we check for sequences of identical chars and then we compute
            // how many chars we need to add to break the sequence into segments
            // of no more than 2 repeating characters
            repeatLength = 1;
            while (i < password.Length - 1 && password[i] == password[i + 1])
            {
                repeatLength++;
                i++;
            }

            if (repeatLength % 2 == 0)
                repeatCount += repeatLength/2 - 1;
            else
                repeatCount += repeatLength/2;

            // we remove the sequence
            if (repeatLength >= 3)
            {
                if (repeatLength % 3 == 0)
                {
                    password = password.Remove(i - repeatLength + 2, repeatLength - 1);
                }
                else if (repeatLength % 3 == 1 && (i < password.Length - 1 && password[i] == password[i + 1]))
                {
                    password = password.Remove(i - repeatLength + 2, repeatLength - 2);
                }
                else if (repeatLength % 3 == 2)
                {
                    password = password.Remove(i - repeatLength + 3, repeatLength - 2);
                }

                repeatCount -= repeatLength / 3;
                i -= repeatLength / 3;
                changes++;
            }
        }

        // if the password legth is < 6, we need to add x=6-legth chars; so we check if
        // x is greater than the number of chars we need to add to meet the rqeuirements
        if (password.Length < 6)
        {
            changes += Math.Max(6 - password.Length,
                repeatCount + (hasLowercase ? 0 : 1) + (hasUppercase ? 0 : 1) + (hasDigit ? 0 : 1));
        }
        // if the password legth is > 6 and < 20, we need to add chars to meet all the requirements
        else if (password.Length <= 20)
        {
            changes += Math.Max(repeatCount, (hasLowercase ? 0 : 1) + (hasUppercase ? 0 : 1) + (hasDigit ? 0 : 1));
        }
        // if the password legth is > 20, we need to delete (20-length) chars
        // and add/update as many as we need to meet the rqeuirements
        else
        {
            int deleteCount = password.Length - 20;
            changes += deleteCount;
            repeatCount = 0;
            for (int i = 0; i < password.Length; i++)
            {
                repeatLength = 1;
                while (i < password.Length - 1 && password[i] == password[i + 1])
                {
                    repeatLength++;
                    i++;
                }

                if (repeatLength % 2 == 0)
                    repeatCount += repeatLength/2 - 1;
                else
                    repeatCount += repeatLength/2;
            }

            changes += Math.Max(repeatCount, (hasLowercase ? 0 : 1) + (hasUppercase ? 0 : 1) + (hasDigit ? 0 : 1));
        }

        return changes;
    }

    public static void Main()
    {
        string myStr;
        myStr = Console.ReadLine();
        Console.WriteLine(MinimumChanges(myStr));
        
        // Console.WriteLine(MinimumChanges("aabc")); -> returns 2
        // Console.WriteLine(MinimumChanges("Password123")); -> returns 0
        // Console.WriteLine(MinimumChanges("aA1aA1aA1aA1aA1aA1aA1aA1aA1aA1aA1")); -> returns 13
    }
}