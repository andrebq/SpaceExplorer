using Godot;
public interface IDockable
{
    void OfferDock(Node node);
    void RevokeDock(Node node);
}