using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestSuite
{
    // Unit test for custom color creation function
    [Test]
    public void TestColorConversion()
    {
        // define hex values for RGB
        int red = 0x40;
        int green = 0x80;
        int blue = 0xC0;

        // create color using hex values
        Color color = PillColors.CreateColor(red, green, blue);

        // assert that resultant floats are equal to what we expect (a fraction of 0xFF)
        Assert.AreEqual(color.r, (float) red / 255);
        Assert.AreEqual(color.g, (float) green / 255);
        Assert.AreEqual(color.b, (float) blue / 255);

    }

    

}
