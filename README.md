# Hot-Pursuit
Hot Pursuit is a Windows 10 desktop application whose purpose is to automate the tracking of a NEO object with TheSkyX.

Targets are input to TheSkyX using the TSXToolKit Transient Search (see rrskybox/Transient Search) application via the NEO Scout search.  The user selects one then launches Hot Pursuit.
Hot Pursuit fetches the name of the current TSX target, queries the CNEOS Scout internet site for its current ephemeral data, slews the mount to the coordinates and changes the tracking to match the target’s pace.  Hot Pursuit recaptures the ephemeral data and resets tracking speeds every few minutes as set by the user.

A detailed description can be found in the "Hot Pursuit Description.docx" within the "Hot Pursuit" folder.
