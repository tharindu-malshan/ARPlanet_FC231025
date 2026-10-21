//using UnityEngine;

//public class Planet : MonoBehaviour
//{

//    public string planetName="Earth";
//    [TextArea(3, 6)]
//    public string planetDescription="Our home planet";
//    factText.text = planet.description;
//}
using UnityEngine;

public class Planet : MonoBehaviour
{
    public string planetName;

    [TextArea(3, 10)] // <-- Make sure this is right above a string variable
    public string fact;
}