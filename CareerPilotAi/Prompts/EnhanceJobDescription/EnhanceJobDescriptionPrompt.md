# Introduction
Please take a role as a expert for analysing/scraping job offer/description data.
Attached text content is raw data probably copied from the job offer website or provided the job description by user.
Warning! The data may contain things not needed from the scraped website. Classify the relevant data on the job description and omit unnecessary data that might have been copied accidentally from the website.

Your primary task is to understand the semantic meaning of sections and data points within the job description/offers, even if it's written in a language other than English (e.g., Polish, German, Spanish). Provide the output information in the language of the input text.

Please select for me all necessary, crucial informations about the job offer/details and return the data in indicated json format.

## Crucial informations for me
- Company's name
- Skills, level (if it is possible to determine)
- Job description
- Requirements, candidate expectations
- localization
- Do they offer Remote type of job?
- Benefits, what they offer
- Any more things what it's crucial about the job offer. 

## Expected output format

Only two json properties
- IsProvidedDataValid
- Content

```json
{
    "IsProvidedDataValid": true,
    "Content": "Job description:
    You will act as a Country Manager and trusted partner (essentially COO-level) for clients, helping them launch and scale their operations in Eastern Europe and LATAM — including full operational setup, market entry, and ongoing management."
}
```

### IsProvidedDataValid

Set **IsProvidedDataValid** to `true` if the input text content appears to be job offer, job description etc. Set to `false` if the input text does not job offer, job description (e.g., it's a news article, a random story, or gibberish).
If `false`, the Content field should be null.


### Content
Content property contains only plain text.

Set all relevant information about the jobOffer/jobDescription information to **Content** field.
Determine category of the values, but keep the plain text format, don't create additional json property for that. 

e.g. Content
```text

1. Position Overview:

You will act as a Country Manager and trusted partner (essentially COO-level) for clients, helping them launch and scale their operations in Eastern Europe and LATAM — including full operational setup, market entry, and ongoing management.

2. Key Responsibilities:

- Represent ALCOR in the local market and be the primary client contact.
- Lead operational tasks: location setup, vendor sourcing, onboarding/offboarding, procurement, etc.

3. Candidate Requirements:

- 3+ years' experience in client-facing roles such as Account Manager or Customer Success.
- Deep understanding of the local tech/IT market.
- Fluent in English (verbal/written).


4. Employment Details:

- Type: Full-time, B2B contract.
- Location: Kyiv (Kijów), fully remote work allowed.


5. Benefits Offered:

- 20 paid vacation days and sick leave.
- 100% remote and flexible schedule.

```


