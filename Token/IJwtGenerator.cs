using NetKubernetes.Models;

namespace NetKubernetes.Token;

public interface  IJwtGenerator {
    string CreatToken(Usuario usuario);
}