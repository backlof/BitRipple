## BitRipple

Download torrent files from RSS feeds to a folder that is watched by your torent client

* Portable
* Simple
* Small intervals

### How it works

The client downloads all your feeds at the time interval of your choosing, and then matches your filter criteria with the feeds. You will get a match when:

- Filter is `Enabled`
- Feed is selected for the filter
- The beginning of the torrent title matches `Filter` (see below)
- Torrent title contains all terms in `Include` (see below)
- Torrent title contains no terms in `Exclude` (see below)

**Additionally, you can**

- `Ignore caps` which affects `Filter`, `Include` and `Exclude`
- `Match once` to deactivate the filter after first successful download (unless it's a TV Show filter)

**For TV shows**

- Torrent episode is equal or higher (only supports format `S{000}E{000}`)
- With `Match once`, each episode will only download once

### Filter

|Symbol	|Meaning											|Regex			|
|:------:|--------------------------------------|:------------:|
|`*`		|Wildcard; zero or more characters.		|`.*`       	|
|`.`		|1 whitespace character 					|`[\s\.\-_]`	|
|`?`		|0 or 1 character of any type.			|`.?`     		|

**Examples**

|Filter				|Torrent								|Match	|
|-----------------|-----------------------------|:------:|
|`TV Show`			|`Bob's TV Show S01E02 720P`	|No		|
|`*TV Show`			|`Bob's TV Show S01E02 720P`	|Yes		|
|`Bob?s TV Show`	|`Bob's TV Show S01E02 720P`	|Yes		|
|`Bob?s TV Show`	|`Bobs TV Show S01E02 720P` 	|Yes		|
|`Bob's.TV.Show`	|`Bob's TV Show S01E02 720P`	|Yes		|
|`Bob's.TV.Show`	|`Bob's.TV.Show.S01E02.720P`	|Yes		|
|`Bob's.TV.Show`	|`Bob's_TV_Show_S01E02_720P`	|Yes		|

### Include and Exclude

- Both are optional
- Separate each term by `;` (empty ones are discarded)
- Use `Ignore caps` if cases vary

**Examples**

|Include			|Exclude	|Torrent											|Match	|
|--------------|--------|--------------------------------------|:------:|
|` `				|`720p`	|`TV Show S01E07 1080p WEBRip DD5.1 `	|Yes		|
|` `				|`1080p`	|`TV Show S01E07 1080p WEBRip DD5.1 `	|No		|
|`1080p;DD5.1`	|` `		|`TV Show S01E07 1080p WEBRip DD5.1 `	|Yes		|
|`WEB;`			|`1080p`	|`TV Show S01E07 1080p WEBRip DD5.1 `	|No		|
|`HDTV;1080p`	|`720p`	|`TV Show S01E07 1080p WEBRip DD5.1 `	|No		|