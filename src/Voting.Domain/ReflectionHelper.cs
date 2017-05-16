
using System.Reflection;

namespace Voting.Domain
{
	public class ReflectionHelper {
		public static Assembly DomainAssembly = typeof(ReflectionHelper).GetTypeInfo().Assembly; 
	}
}