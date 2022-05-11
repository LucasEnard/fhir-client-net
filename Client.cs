﻿//----------------------------------------------------------------------------------------------------------
// Imports
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;

//----------------------------------------------------------------------------------------------------------
// Part 1

// Creation of an htpclient holding the api key of the server as an header
var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Add("x-api-key", "sVgCTspDTM4iHGn51K5JsaXAwJNmHkSG3ehxindk");

var settings = new FhirClientSettings
    {
        Timeout = 0,
        PreferredFormat = ResourceFormat.Json,
        VerifyFhirVersion = true,
        PreferredReturn = Prefer.ReturnRepresentation
    };
// Creation of our client using the right url
var client = new FhirClient("https://fhir.8ty581k3dgzj.static-test-account.isccloud.io",httpClient,settings);

//----------------------------------------------------------------------------------------------------------
// Part 2

// Building a new patient and setting the names
var patient0 = new Patient();
patient0.Name.Add(new HumanName().WithGiven("GivenName").AndFamily("FamilyName"));

// Creation of our client in the server
var created_pat = client.Create<Patient>(patient0);

Console.Write("Part 2 : Newly created patient id : ");
Console.WriteLine(created_pat.Id);

//----------------------------------------------------------------------------------------------------------
// Part 3

// Get the patient by reading our server using our id ( the id we got from the creation )
// patient0 = client.Read<Patient>("Patient/" + created_pat.Id);

// Get the patient by searching our server using the name of our patient

// This gets all the Patient having the exact name "FamilyName" and we take the first one
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

// Creation of our observatiob in the server
var new_obsv = client.Create<Observation>(obsv);

Console.Write("Part 4 : Id of the observation : ");
Console.WriteLine(new_obsv.Id);
