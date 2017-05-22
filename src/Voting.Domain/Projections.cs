
namespace Voting.Domain
{
    public static class Projections
    {
        public const string Voting = @"
        fromCategory('VotingAggregate')
        .when({
            $init : function(state, event)
            {
                return { 
                }
            },
            $any: function(state, ev){
                linkTo('Voting', ev);
                return state;          
            }      
        });";
    }
}