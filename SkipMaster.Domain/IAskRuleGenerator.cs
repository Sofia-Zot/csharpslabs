namespace SkipMaster.Domain;

// на старте семестра каждый препод получает свое правило
public interface IAskRuleGenerator
{
    IAskRule Generate(Subject ownSubject);
}
