# Revit AI — Early Experiments

An archive of my early experiments integrating AI-related workflows into Autodesk Revit. These projects show the evolution of the idea, including unfinished features and design choices I would approach differently today.

## Projects

### 00_RevitAPI_AI_Plugin

A Revit 2022 dockable chat panel with an OpenAI API-backed conversation prototype and locally routed Revit API commands. The prototype reads `OPENAI_API_KEY` from an environment variable. It is **not** a fully autonomous AI agent. Some command handlers are incomplete or not connected to the chat workflow.

### RevitAPI_AI_HUB_Plugin

A local, keyword-based catalogue and plugin recommendation prototype. It maps a typed request to a listed Revit add-in, but **does not automatically execute** the suggested add-in, and does not call an AI model.

## Why share this?

These projects document earlier attempts to make a collection of specialized Revit tools accessible through a unified interface. Later experiments explored Python scripting and MCP, but the current Solo Hub implementation is **not included here**.

## Important limitations

- **Historical source archive, not production-ready software.** The code was not compiled or tested as part of this publication.
- Original projects reference separately maintained `ZimovetsPluginLibrary` and `ZimovetsPluginLibrary.AI`, which are **not included**. The preserved Visual Studio solutions may not build on their own.
- Paths in the historical project configurations may need adaptation to another development machine.
- Test only on disposable Revit models. Some experimental Revit API operations need further safety and threading review.
- The API connection requires your own separately configured credentials and may incur charges. Never commit tokens or keys.

No credentials or personal API setup files were intentionally transferred from the original private repository. This archive has not undergone a complete security audit.

## Licensing

No license is granted at this time. The repository is publicly viewable for learning and discussion, but reuse and redistribution require permission.
