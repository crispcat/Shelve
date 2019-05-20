namespace Shelve.Tests
{
    using Shelve.IO;
    using Shelve.Core;
    using NUnit.Framework;

    [TestFixture]
    public class InternalVariableSetTests
    {
        [Test] public void SingleSet()
        {
            var parsedSet = JsonPacker.ExtractDataAs<ParsedSet>(@"

            {
                'Name' : 'LinearMove',
                'Declares' :
                {
                    'initial' :
                    [
                        'position = 0',
                        'A = 0',
                        'V = 1'
                    ],
                    'timeStep' : 
                    [
                        'T += [0, 1]'
                    ]
                },
                'Expressions':
                [
                    'V += A * T',
                    'distance += position + V * T'
                ]
            }"

            );

            var linearMove = new SetTranslator(parsedSet).Translate();

            linearMove.__("A").Value = 4;

            var time = linearMove.Get<Iterator>("T");

            while (linearMove.__("distance").Value <= 20)
            {
                time.MoveNextValue();
            }

            Assert.IsTrue(linearMove.__("T").Value == 3);
        }

        [Test] public void Merges()
        {
            var parsedSet1 = JsonPacker.ExtractDataAs<ParsedSet>(@"

            {
                'Name' : 'Forest',
                'Declares' :
                {
                    'timeStepOneDay' : 
                    [
                        'day += [0, 1]'
                    ]
                }
            }

            ");

            var parsedSet2 = JsonPacker.ExtractDataAs<ParsedSet>(@"

            {
                'Name' : 'Grass',
                'Uses' : ['Forest'],
                'Declares' :
                {
                    'initialGrass' :
                    [
                        'initialGrassPopulation = 15'
                    ],

                    'factorsGrass' :
                    [
                        'grassGrowSpeed = 3'
                    ]
                },
                'Expressions' :
                [
                    'grassPopulation += initialGrassPopulation + day * grassGrowSpeed'
                ]
            }

            ");

            var forest = new SetTranslator(parsedSet1).Translate();
            var grass = new SetTranslator(parsedSet2).Translate();

            try
            {
                var grassPopulation = grass.__("grassPopulation").Value;
                Assert.Fail();
            }
            catch { }

            forest.Merge(grass);

            forest.Get<Iterator>("day").MoveNextValue();

            Assert.IsTrue(forest.__("grassPopulation").Value == 18);
        }
    }
}
