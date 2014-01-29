ByChance
========

The ByChance Framework allows game developers to provide an infinite amount of unique levels for both 2D and 3D games. Easy to integrate into video games of all genres, ByChance enables you to generate complex levels including all of their game components and comes with useful post-processing algorithms that can be applied afterwards in order to ensure a great gaming experience.


## Getting Started

The core of the ByChance Framework is a generic level generation algorithm that is able to construct 2D and 3D levels alike. Thus, you only need to understand the framework fundamentals once, and will then be able to create levels for games of all genres.

ByChance is a .NET Framework class library written in C# that follows the .NET patterns and idoms presented in the book *Framework Design Guidelines* by Cwalina, Abrams et al. This page will explain the basics of integrating the ByChance Framework for creating random levels in your games. All code samples are written in C#.

### Getting ByChance Binaries

The easiest way of integrating ByChance in your project is grabbing the [latest binaries](https://github.com/npruehs/ByChance/tree/master/Release/Latest) from GitHub. Just download the library and reference it in your project.

### Getting ByChance Sources

ByChance is open source under the MIT license. Feel free to get the [latest sources](https://github.com/npruehs/ByChance/tree/master/Source) from GitHub and add all files to your project.

## Generating Your First Level

ByChance thinks of a game level as a bounded space consisting of a limited number of level building blocks called *chunks*. Each chunk contains information about its extents, its position and rotation as well as about where to align it to the existing level and where to add game elements like enemies, items or levers.

The only thing you need to do before generating your first level is to setup a *chunk library*. This chunk library holds a set of *chunk templates* that is used by the level generator for generating the game levels. For 2D levels this could look like this:

```csharp
ChunkLibrary2D chunkLibrary = new ChunkLibrary2D();
```

After having instantiated the chunk library, you need to add chunk templates to be used by the level generator. For 2D levels, you have to specify at least the width and height of these templates:

```csharp
ChunkTemplate2D chunkTemplate = new ChunkTemplate2D(new Vector2F(30f, 50f));
```

Next, the level generator needs to know how chunks constructed with this template can be put together. ByChance uses to concept of *contexts* for specifying where chunks can be aligned. Every chunk has to contain at least one single context describing the relative position at which it may be aligned to other chunks:

```csharp
chunkTemplate.AddContext(new Vector2F(15f, 0f));
chunkTemplate.AddContext(new Vector2F(15f, 50f));
```

Now, our chunk template has two contexts, one at the top-center and one at the bottom-center. Note that while the framework will work with a chunk library that contains only one chunk template, the resulting level is sure to be dull. The more choices the framework has in regard to chunk templates, the more interesting the final result will be.

As soon as the chunk library has been properly set up, it can be passed to the level generator, along with the desired level size:

```csharp
LevelGenerator2D levelGenerator = new LevelGenerator2D();
Level2D level = levelGenerator.GenerateLevel(chunkLibrary, new Vector2F(800f, 600f));
```

After this, you can access the content of the resulting level by using the accessor methods of the *Level* class:

```csharp
foreach (Chunk2D chunk in level)
{
    // Do something with the chunk.
}
```

## Customizing the Chunk Library

### Weights and Tags

Every chunk has a relative weight that tells the level generator how often a specific chunk should be added to the level. A chunk with a weight of 3 is added to the level about three times as often as a chunk with a weight of 1. The easiest way to specify these weights is by passing them to the constructor for chunk templates:

```csharp
ChunkTemplate2D chunkTemplate = new ChunkTemplate2D(new Vector2F(30f, 50f), 3);
```

Additionally, chunks and contexts can be tagged in order to enable to definition of domain-specific rules, such as whether two given contexts can be aligned at all. This allows attaching treasure rooms to boss rooms, for example, and will be discussed in detail later.

```csharp
ChunkTemplate2D chunkTemplate = new ChunkTemplate2D(new Vector2F(30f, 50f), "boss");
```

### Anchors

Every chunk may contain one or more tagged *anchors* describing the relative position at which game elements can be added:

```csharp
chunkTemplate.AddAnchor(new Vector2F(10f, 10f), "treasure");
chunkTemplate.AddAnchor(new Vector2F(20f, 20f), "stairs");
```

### Chunk Rotations

Apart from assigning contexts and anchors to chunk templates, the ByChance Framework also offers the option to allow the rotation of chunks during the level generation process. This can be very useful when defining floor or corner chunks and can keep the number of chunk definitions at a minimum.

```csharp
ChunkTemplate2D chunkTemplate = new ChunkTemplate2D(new Vector2F(30f, 50f), true);
```

ByChance always rotates by 90° in order to reduce the number of iterations. 3D chunks are always rotated by 90° around the y-axis. If the chunk has been rotated by 360° around the y-axis, it is rotated by 90° around the x-axis after. If the chunk has been rotated by 360° around the x-axis, it is rotated by 90° around the z-axis after. This leads to a total of 64 possible rotations of each 3D chunk. You might want to consider adding rotated versions of the chunk to the chunk library instead in order to trade memory for running time.

## Setting The First Level Chunk

In some cases, you don’t want the framework to pick the first level chunk for you. Let’s assume that you want to place a crossing chunk at the level center, and that this chunk is the first one in your chunk library. Then you can set this chunk as initial level chunk as follows:

```csharp
// Create empty level.
Vector2F levelExtents = new Vector2F(800f, 600f);
Level2D level = new Level2D(levelExtents);

// Set starting chunk.
Chunk2D startingChunk = new Chunk2D(chunkTemplate);
Vector2F chunkPosition = (levelExtents - startingChunk.Extents) / 2;

level.SetStartingChunk(startingChunk, chunkPosition);

// Generate level.
LevelGenerator2D levelGenerator = new LevelGenerator2D();
levelGenerator.GenerateLevel(chunkLibrary, level);
```

## Configuring the Level Generator

For more advanced scenarios, the level generator can be configured with additional parameters. First, you need to import the configuration namespace of the framework:

```csharp
using ByChance.Configuration;
```

Now, you can access the level generator configuration through the LevelGenerator.Configuration property. The following sections summarize the possiblities of customizing the level generation process.


### Restricting Context Alignment

If your chunk library contains many chunks with very similar extents, this can lead to boring repetitive patterns. In the Angry Bots Infinite showcase, we decided to tag door contexts in order to avoid artifacts of this kind: Doors that were intended to lead to other rooms were prevented from being aligned to each other.

This is done by implementing the IContextAlignmentRestriction interface and setting the corresponding property of the level generator configuration:

```csharp
public class DoorContextAlignmentRestriction : IContextAlignmentRestriction
{
    public bool CanBeAligned(Context first, Context second)
    {
        return !(first.Tag.Equals("Door") && second.Tag.Equals("Door"));
    }
}
```

```csharp
levelGenerator.Configuration.ContextAlignmentRestriction = new DoorContextAlignmentRestriction();
```

### Post-processing

Since it is the nature of the level generation algorithm to fill out the level boundaries as much as possible, the resulting level often shows unwanted patterns. A typical example is a corridor that leads nowhere. To avoid this, some kind of post-processing is required after the level has been generated.

We implemented a way to run through an arbitrary number of postprocessing steps called *policies* that can greatly improve the layout of the levels. Each level generator holds a list of those policies which is empty by default. You can add several policies to your level generator instance, which are called directly after the level generation process in the order in which they were added.

#### Aligning Adjacent Contexts

The first framework policy checks all open contexts in the level and connects pairs of contexts that are within a certain offset to each other, specified via the constructor of the policy. The probability of this to occur increases if your chunks have similar dimensions and their contexts have similar relative positions.

```csharp
AlignAdjacentContextsPolicy policy = new AlignAdjacentContextsPolicy(0.1f);
levelGenerator.Configuration.PostProcessingPolicies.Add(policy);
```

#### Discarding Open Chunks

The second one finds all chunks within the level that have open contexts and discards them. As deleting chunks opens up previously aligned contexts of neighbouring chunks, this process is repeated until the first iteration in which no chunk is discarded.

```csharp
DiscardOpenChunksPolicy policy = new DiscardOpenChunksPolicy();
levelGenerator.Configuration.PostProcessingPolicies.Add(policy);
```

You can restrict which chunks to discard by specifying your own implementation of IDiscardOpenChunksRestriction for the policy:

```csharp
public class DiscardOpenFloorsRestriction : IDiscardOpenChunksRestriction
{
    public bool ShouldBeDiscarded(Chunk chunk)
    {
        // Discard open floors.
        return chunk.Tag.Equals("Floor");
    }
}
```

```csharp
DiscardOpenChunksPolicy policy = new DiscardOpenChunksPolicy();
policy.DiscardOpenChunksRestriction = new DiscardOpenFloorsRestriction();
levelGenerator.Configuration.PostProcessingPolicies.Add(policy);
```

#### Discarding Open Contexts

The third framework policy cleans up all open contexts. This policy is useful for discarding unused contexts before drawing the level or performing further operations on it.

```csharp
DiscardOpenContextsPolicy policy = new DiscardOpenContextsPolicy();
levelGenerator.Configuration.PostProcessingPolicies.Add(policy);
```

Just like for open chunks, you can specify which open contexts to discard by providing your own implementation of IDiscardOpenContextsRestriction:

```csharp
public class DiscardOpenDoorsRestriction : IDiscardOpenContextsRestriction
{
    public bool ShouldDiscardContext(Context context)
    {
        // Discard doors that are leading nowhere.
        return context.Tag.Equals("Door");
    }
}
```

```csharp
DiscardOpenContextsPolicy policy = new DiscardOpenContextsPolicy();
policy.DiscardOpenContextsRestriction = new DiscardOpenDoorsRestriction();
levelGenerator.Configuration.PostProcessingPolicies.Add(policy);
```

#### Creating Custom Post-Processing Policies

You can easily add further policies by implementing the IPostProcessingPolicy interface provided by the framework.

