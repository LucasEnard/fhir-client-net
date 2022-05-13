//----------------------------------------------------------------------------------------------------------
// Imports
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;

//----------------------------------------------------------------------------------------------------------
// Part 1

// Creation of an htpclient holding the api key of the server as an header
var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Add("x-api-key", "api-key");


var settings = new FhirClientSettings
    {
        Timeout = 0,
        PreferredFormat = ResourceFormat.Json,
        VerifyFhirVersion = true,
        // PreferredReturn can take Prefer.ReturnRepresentation or Prefer.ReturnMinimal to return the full resource or an empty payload
        PreferredReturn = Prefer.ReturnRepresentation
    };
// Creation of our client using the right url
var client = new FhirClient("url",httpClient,settings);

//----------------------------------------------------------------------------------------------------------
// Part 2

// Building a new patient and setting the names
var patient0 = new Patient();
patient0.Name.Add(new HumanName().WithGiven("GivenName").AndFamily("FamilyName"));

// Creation of our client in the server
// It is to be noted that using SearchParams you can check if an equivalent resource already exists in the server
// For more information https://docs.fire.ly/projects/Firely-NET-SDK/client/crud.html
var created_pat = client.Create<Patient>(patient0);

Console.Write("Part 2 : Newly created patient id : ");
Console.WriteLine(created_pat.Id);

//----------------------------------------------------------------------------------------------------------
// Part 3

// Get the patient by reading our server using our id ( the id we got from the creation )
// patient0 = client.Read<Patient>("Patient/" + created_pat.Id);

// Get the patient by searching our server using the name of our patient

// This gets all the Patient having the exact name "FamilyName" and we take the first one
// Note that if you have multiple patients with the same name, you will get only the first one 
// We advise to use the id, the names, and any other informations for the SearchParams to be sure to get the right patient
var q = new SearchParams().Where("name:exact=FamilyName");
Bundle bund = client.Search<Patient>(q);
patient0 = bund.Entry[0].Resource as Patient;

Console.Write("Part 3 : Name of the patient we found by searching : ");
Console.WriteLine(patient0.Name[0]);


// Creation of our patient telecom, here a phone number
patient0.Telecom.Add(new ContactPoint(new ContactPoint.ContactPointSystem(),new ContactPoint.ContactPointUse(),"1234567890"));

// Change the given name of our patient
patient0.Name[0].Given = new List<string>() { "AnotherGivenName" };

Console.Write("Part 3 : Name of the changed patient : ");
Console.WriteLine(patient0.Name[0]);

Console.Write("Part 3 : Phone of the changed patient : ");
Console.WriteLine(patient0.Telecom[0].Value);

// Update the patient
var update_pat = client.Update<Patient>(patient0);

//----------------------------------------------------------------------------------------------------------
// Part 4

// Building of our new observation
Observation obsv = new Observation {

    Value = new Quantity(70, "kg"),
    Code = new CodeableConcept {
        Coding = new List<Coding> {
            new Coding {
                System = "http://loinc.org",
                Code = "29463-7",
                Display = "Body weight"
            }
        }},
    Category = new List<CodeableConcept> {
        new CodeableConcept {
            Coding = new List<Coding> {
                new Coding {
                    System = "http://snomed.info/sct",
                    Code = "276327007",
                    Display = "Body weight"
                }
            }
        }},
    Status = new ObservationStatus {},
    Subject = new ResourceReference {
        Reference = "Patient/" + update_pat.Id}

    };

// Creation of our observation in the server
var new_obsv = client.Create<Observation>(obsv);

Console.Write("Part 4 : Id of the observation : ");
Console.WriteLine(new_obsv.Id);
