using System;
using System.Collections.Generic;
using System.Linq;
using Dolittle.SDK.Concepts;
using Dolittle.SDK.Events;
using Dolittle.Vanir.Backend.GraphQL;

namespace Backend
{
    public class QuoteId : ConceptAs<Guid>
    {
        public static QuoteId Empty { get; } = Guid.Empty;

        public QuoteId(Guid guid) => Value = guid;

        public QuoteId() => Value = Guid.Empty;

        public static QuoteId New() => new QuoteId(Guid.NewGuid());

        public static implicit operator QuoteId(Guid value) => new QuoteId(value);

        public static implicit operator QuoteId(EventSourceId value) => new QuoteId(value);

        public static implicit operator EventSourceId(QuoteId value) => new EventSourceId { Value = value };
    }

    public class CustomerId : ConceptAs<Guid>
    {
        public static CustomerId Empty { get; } = Guid.Empty;
        public CustomerId() => Value = Guid.Empty;
        public CustomerId(Guid guid) => Value = guid;
        public static CustomerId New() => new CustomerId(Guid.NewGuid());
        public static implicit operator CustomerId(Guid value) => new CustomerId(value);
        public static implicit operator CustomerId(EventSourceId value) => new CustomerId(value);
        public static implicit operator EventSourceId(CustomerId value) => new EventSourceId { Value = value };
    }

    public class BasicCustomerDetails : Value<BasicCustomerDetails>
    {
        public DistributorId DistributorId { get; set; }
        public NationalIdentityNumber NationalIdentityNumber { get; set; }
        public Name Name { get; set; }
        public Sex Sex { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
    }

    public class DistributorId : ConceptAs<Guid>
    {
        public static DistributorId Empty { get; } = Guid.Empty;
        public static DistributorId New() => new DistributorId(Guid.NewGuid());

        public DistributorId(Guid guid) => Value = guid;

        public DistributorId()
        {
        }

        public static implicit operator DistributorId(Guid value) => new DistributorId(value);

        public static implicit operator DistributorId(EventSourceId value) => new DistributorId(value);

        public static implicit operator EventSourceId(DistributorId value) => new EventSourceId { Value = value };
    }

    public class GivenName : ConceptAs<string>
    {
        public static GivenName Empty { get; } = string.Empty;

        public GivenName(string val) => Value = val;

        public GivenName() => Value = string.Empty;

        public static implicit operator GivenName(string value) => new GivenName(value);
    }

    public class MiddleName : ConceptAs<string>
    {
        public static MiddleName Empty { get; } = string.Empty;

        public MiddleName(string val) => Value = val;

        public MiddleName() => Value = string.Empty;

        public static implicit operator MiddleName(string value) => new MiddleName(value);
    }

    public class FamilyName : ConceptAs<string>
    {
        public static FamilyName Empty { get; } = string.Empty;
        public FamilyName(string val) => Value = val;
        public FamilyName() => Value = string.Empty;
        public static implicit operator FamilyName(string value) => new FamilyName(value);
    }

    public class Name : Value<Name>
    {

        public GivenName GivenName { get; set; }

        public MiddleName MiddleName { get; set; }
        public FamilyName FamilyName { get; set; }

        public string FullName
        {
            get
            {
                if (MiddleName == MiddleName.Empty)
                {
                    return $"{GivenName} {FamilyName}".Trim();
                }

                return $"{GivenName} {MiddleName} {FamilyName}".Trim();
            }
        }
    }

    public enum Sex
    {
        Unspecified = 0,
        Male = 1,
        Female = 2
    }

    public class IsSmoker : ConceptAs<bool>
    {
        public static implicit operator IsSmoker(bool value)
        {
            return new IsSmoker { Value = value };
        }
    }

    public class IsMarriedOrCohabiting : ConceptAs<bool>
    {
        public static implicit operator IsMarriedOrCohabiting(bool value)
        {
            return new IsMarriedOrCohabiting { Value = value };
        }
    }

    public enum EducationLevel
    {
        Unknown = 0,
        Basic = 1,
        Further = 2,
        Higher = 3
    }


    public enum EmploymentSector
    {
        Unknown = 0,
        Office = 1,
        IndustryManufacturingAndCrafts = 2,
        Transportation = 3,
        AgricultureAndForestry = 4,
        TradeAndService = 5,
        Teaching = 6,
        Healthcare = 7,
        Hospitality = 8,
        PoliceSecurityFireAndDefence = 9,
        FishingOffshoreAndDiving = 10,
        FulltimeStudent = 11,
        Sport = 12,
        CultureMediaAndResearch = 13
    }

    public class Money : Value<Money>
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }

        public Money()
        {
        }

        public Money(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }

        public Money(double value, string currency)
        {
            Amount = Convert.ToDecimal(value);
            Currency = currency;
        }

        public Money(int value, string currency)
        {
            Amount = Convert.ToDecimal(value);
            Currency = currency;
        }

        public static Money operator +(Money a, Money b) => new Money(a.Amount + b.Amount, a.Currency);
        public static Money operator +(Money a, int b) => new Money(a.Amount + b, a.Currency);

        public static Money operator -(Money a, Money b) => new Money(a.Amount - b.Amount, a.Currency);
        public static Money operator *(Money a, Money b) => new Money(a.Amount * b.Amount, a.Currency);
        public static Money operator *(Money a, int b) => new Money(a.Amount * b, a.Currency);

        // TODO: burde nok kaste exception ved ulik currency
        public static bool operator >=(Money a, Money b) => a.Amount >= b.Amount;

        // TODO: burde nok kaste exception ved ulik currency
        public static bool operator <=(Money a, Money b) => a.Amount <= b.Amount;
    }

    public class NonChildBenefit
    {
        public BenefitWithRecommendation Benefit { get; set; }
        public bool IsSelected { get; set; }
    }


    public enum BenefitType
    {
        NotSet = 0,
        Death = 1,
        Disability = 2,
        CriticalIllness = 3,
        ChildrenBasic = 4,
        ChildrenExtra = 5,
        ChildrenSuper = 6
    }


    public class BenefitId : ConceptAs<Guid>
    {
        public static BenefitId Empty { get; } = Guid.Empty;

        public BenefitId(Guid guid) => Value = guid;

        public BenefitId() => Value = Guid.Empty;

        public static BenefitId New() => new BenefitId(Guid.NewGuid());

        public static implicit operator BenefitId(Guid value) => new BenefitId(value);

        public static implicit operator BenefitId(EventSourceId value) => new BenefitId(value);

        public static implicit operator EventSourceId(BenefitId value) => new EventSourceId { Value = value };
    }

    public class RecommendationReasons : Value<RecommendationReasons>
    {
        public static RecommendationReasons None() => new RecommendationReasons();

        public RecommendationReasons()
        {
            Solution = string.Empty;
            Advantages = string.Empty;
            TextToAppendCoverage = string.Empty;
            Effects = string.Empty;
        }

        public string Solution { get; set; }
        public string Advantages { get; set; }
        public string Effects { get; set; }
        public string TextToAppendCoverage { get; set; }
    }

    public class NationalIdentityNumber : ConceptAs<string>
    {
        bool _isValid;

        public static NationalIdentityNumber Empty { get; } = string.Empty;

        public NationalIdentityNumber(string val)
        {
            Value = val;
        }
        public NationalIdentityNumber() => Value = string.Empty;

        public static implicit operator NationalIdentityNumber(string value) => new NationalIdentityNumber(value);
    }


    public class BenefitWithRecommendation : Value<BenefitWithRecommendation>
    {
        public BenefitId Id { get; set; }
        public BenefitType Type { get; set; }
        public Money Premium { get; set; }
        public Money Coverage { get; set; }
        public Money RecommendedCoverage { get; set; }
        public Money RecommendedCoveragePremium { get; set; }
        public Money MinimumCoverage { get; set; }
        public Money MaximumCoverage { get; set; }
        public RecommendationReasons Reasons { get; set; }
        public BenefitType RecommendedBenefitType { get; set; }
        public NationalIdentityNumber ChildSSN { get; set; }

        public BenefitWithRecommendation(
            BenefitId id,
            BenefitType type,
            Money premium,
            Money coverage,
            Money recommendedCoverage,
            Money recommendedCoveragePremium,
            Money maximumCoverage,
            Money minimumCoverage,
            RecommendationReasons reasons,
            BenefitType recommendedBenefitType = BenefitType.NotSet,
            NationalIdentityNumber childSSN = null)
        {
            Id = id;
            Type = type;
            Premium = premium;
            Coverage = coverage;
            RecommendedCoverage = recommendedCoverage;
            RecommendedCoveragePremium = recommendedCoveragePremium;
            MaximumCoverage = maximumCoverage;
            MinimumCoverage = minimumCoverage;
            Reasons = reasons ?? RecommendationReasons.None();
            RecommendedBenefitType = BenefitType.ChildrenSuper;
            ChildSSN = childSSN ?? NationalIdentityNumber.Empty;
        }
    }

    public class ChildBenefits
    {
        public IEnumerable<BenefitWithRecommendation> Benefits { get; set; }

        public bool IsRecommended => Benefits?.Any(_ => _.RecommendedBenefitType == _.Type) ?? false;

        public BenefitType BenefitType => Benefits.FirstOrDefault()?.Type ?? BenefitType.NotSet;

        public bool HasBenefits => Benefits?.Any() ?? false;

        public Money Total => new Money(Benefits.Sum(_ => _.Premium.Amount), "NOK");

        public bool IsSelected { get; set; }
    }

    public class CustomerBenefits
    {
        public static readonly CustomerBenefits None = new CustomerBenefits
        {
            NonChildBenefits = new List<NonChildBenefit>(),
            ChildBenefits = new List<ChildBenefits>()
        };

        public IEnumerable<NonChildBenefit> NonChildBenefits { get; set; }

        public IEnumerable<ChildBenefits> ChildBenefits { get; set; }

        //Shouldn't be hardcoded to NOK but ok for now.
        public Money Total => new Money(0, "NOK");

        decimal TotalFromNonChildBenefits()
        {
            return NonChildBenefits?.Where(_ => _.IsSelected).Sum(_ => _.Benefit?.Premium.Amount) ?? 0;
        }

        decimal TotalFromChildBenefits()
        {
            return ChildBenefits.FirstOrDefault(_ => _.IsSelected)?.Total.Amount ?? 0;
        }
    }


    public enum ChangeOriginator
    {
        Unknown = 0,
        Agent = 1,
        User = 2,
        Caseworker = 3,
        Integration = 4,
        System = 5
    }

    public class SourceOfChange
    {
        public ChangeOriginator Originator { get; set; }
        public Guid SourceId { get; set; }
        public string Application { get; set; }
    }

    public class EmailAddress : ConceptAs<string>
    {

        public EmailAddress(string value) => Value = value;

        public EmailAddress() => Value = string.Empty;

        public static implicit operator EmailAddress(string value) =>
            new EmailAddress(value);
    }

    public class Sent : Value<Sent>
    {
        public EmailAddress To { get; set; }

        public DateTimeOffset At { get; set; }
    }

    public class OfferId : ConceptAs<Guid>
    {
        public static OfferId Empty { get; } = Guid.Empty;

        public OfferId(Guid guid) => Value = guid;

        public OfferId() => Value = Guid.Empty;

        public static OfferId New() => new OfferId(Guid.NewGuid());

        public static implicit operator OfferId(Guid value) =>
            new OfferId(value);

        public static implicit operator OfferId(string value) =>
            string.IsNullOrWhiteSpace(value)
                ? Empty
                : new OfferId(Guid.Parse(value));

        public static implicit operator OfferId(EventSourceId value) =>
            new OfferId(value);

        public static implicit operator EventSourceId(OfferId value) =>
            new EventSourceId { Value = value };
    }

    public class Quote
    {
        public Quote()
        {
            Sendings = new Sent[0];
        }

        public QuoteId Id { get; set; }
        public CustomerId ForCustomer { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset LastUpdated { get; set; }
        public BasicCustomerDetails CustomerDetails { get; set; }
        public IsSmoker IsSmoker { get; set; }
        public IsMarriedOrCohabiting IsMarriedOrCohabiting { get; set; }
        public EducationLevel Education { get; set; }
        public EmploymentSector Employment { get; set; }
        public Money Income { get; set; }
        public Money Debt { get; set; }
        public CustomerBenefits CustomerBenefits { get; set; }
        public IEnumerable<BenefitWithRecommendation> DeselectedCustomerBenefits { get; set; }
        public SourceOfChange SourceOfChange { get; set; }
        public IEnumerable<Sent> Sendings { get; set; }

        public bool HasBeenSent => Sendings?.Any() ?? false;

        /// <summary>
        /// Set once this quote has been turned into a quote
        /// </summary>
        public OfferId TurnedIntoOfferId { get; set; }

        /// <summary>
        /// Set to true once the offer
        /// </summary>
        public bool OfferAccepted { get; set; } = false;
    }


    public class SomeQueries : GraphController
    {
        [Query]
        public Quote GetQuote()
        {
            return new Quote
            {
                Id = Guid.NewGuid(),
                ForCustomer = Guid.NewGuid(),
                Created = DateTimeOffset.Now,
                LastUpdated = DateTimeOffset.Now,
                CustomerDetails = new BasicCustomerDetails
                {
                },
                IsSmoker = true,
                IsMarriedOrCohabiting = true,
                Education = EducationLevel.Basic,
                Employment = EmploymentSector.Office,
                Income = new Money(50, "NOK"),
                Debt = new Money(25, "NOK"),
                CustomerBenefits = new CustomerBenefits
                {
                },
                DeselectedCustomerBenefits = new BenefitWithRecommendation[0],
                SourceOfChange = new SourceOfChange(),
                Sendings = new[] {
                    new Sent { To = "Hallois "}
                },
                TurnedIntoOfferId = Guid.NewGuid(),
                OfferAccepted = true
            };
        }
    }
}
